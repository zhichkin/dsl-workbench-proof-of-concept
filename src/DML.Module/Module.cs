﻿using OneCSharp.AST.Model;
using OneCSharp.AST.UI;
using OneCSharp.DML.Model;
using OneCSharp.MVVM;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace OneCSharp.DML.Module
{
    public sealed class Module : IModule
    {
        #region " String resources "
        public const string ONE_C_SHARP = "ONE-C-SHARP";
        #endregion

        public Module() { }
        public T GetService<T>()
        {
            throw new NotImplementedException();
        }
        public T GetProvider<T>()
        {
            throw new NotImplementedException();
        }
        public T GetController<T>()
        {
            throw new NotImplementedException();
        }
        public IController GetController(Type type)
        {
            throw new NotImplementedException();
        }
        public IShell Shell { get; private set; }
        public void Initialize(IShell shell)
        {
            Shell = shell ?? throw new ArgumentNullException(nameof(shell));
            
            RegisterScopeProviders();

            try
            {
                // TODO: move to the base abstract module class !
                RegisterConceptLayouts();
            }
            catch (Exception ex)
            {
                // TODO: log error !!!
                MessageBox.Show("Failed to load DML module:"
                    + Environment.NewLine
                    + Environment.NewLine
                    + ex.Message,
                    ONE_C_SHARP, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
        private void RegisterConceptLayouts()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            string modulePath = Path.GetDirectoryName(assembly.Location);
            string moduleName = Path.GetFileName(assembly.Location);
            string layoutsPath = moduleName.Replace("Module", "UI");
            string conceptsPath = moduleName.Replace("Module", "Model");
            layoutsPath = Path.Combine(modulePath, layoutsPath);
            conceptsPath = Path.Combine(modulePath, conceptsPath);

            if (File.Exists(layoutsPath) && File.Exists(conceptsPath))
            {
                Assembly layouts = Assembly.LoadFrom(layoutsPath);
                Assembly concepts = Assembly.LoadFrom(conceptsPath);
                foreach (Type concept in concepts.GetTypes().Where(t => t.IsSubclassOf(typeof(SyntaxNode))))
                {
                    foreach (Type layout in layouts.GetTypes()
                        .Where(t => t.GetInterfaces()
                                    .Where(i => i == typeof(IConceptLayout))
                                    .Count() > 0))
                    {
                        if (layout.BaseType.IsGenericType
                            && layout.BaseType.GetGenericArguments()
                                .Where(t => t == concept).Count() > 0)
                        {
                            SyntaxTreeController.Current.RegisterConceptLayout(
                                concept,
                                (IConceptLayout)Activator.CreateInstance(layout));
                            break;
                        }
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("DML module files not found!");
            }
        }
        private void RegisterScopeProviders()
        {
            SyntaxTreeManager.RegisterScopeProvider(typeof(VariableConcept), new VariableScopeProvider());
            SyntaxTreeManager.RegisterScopeProvider(typeof(ParameterConcept), new ParameterScopeProvider());
            SyntaxTreeManager.RegisterScopeProvider(typeof(ColumnConcept), new ColumnReferenceScopeProvider());
        }
    }
}