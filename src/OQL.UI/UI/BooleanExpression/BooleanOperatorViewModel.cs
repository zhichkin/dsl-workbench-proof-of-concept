using Microsoft.VisualStudio.PlatformUI;
using OneCSharp.OQL.Model;
using System;
using System.Windows.Input;

namespace OneCSharp.OQL.UI
{
    public sealed class BooleanOperatorViewModel : SyntaxNodeViewModel
    {
        public BooleanOperatorViewModel(ISyntaxNodeViewModel parent, BooleanOperator model) : base(parent, model)
        {
            this.ChangeOperatorTypeCommand = new DelegateCommand<string>(this.ChangeOperatorType);
            this.AddComparisonOperatorCommand = new DelegateCommand(this.AddComparisonOperator);
            this.AddBooleanOperatorCommand = new DelegateCommand(this.AddInnerBooleanOperator);
            this.RemoveOperatorCommand = new DelegateCommand(this.RemoveOperator);

            InitializeViewModel();
        }
        public override void InitializeViewModel()
        {
            Operands = new SyntaxNodeListViewModel(this, ((BooleanOperator)Model).Operands);

            foreach (var operand in ((BooleanOperator)Model).Operands)
            {
                if (operand is BooleanOperator)
                {
                    Operands.Add(new BooleanOperatorViewModel(this, (BooleanOperator)operand));
                }
                else if (operand is ComparisonOperator)
                {
                    Operands.Add(new ComparisonOperatorViewModel(this, (ComparisonOperator)operand));
                }
            }
        }
        public string Keyword
        {
            get { return ((BooleanOperator)Model).Keyword; }
            set { ((BooleanOperator)Model).Keyword = value; OnPropertyChanged(nameof(Keyword)); }
        }
        public SyntaxNodeListViewModel Operands { get; set; }
        public int OperandsBorderThickness
        {
            get
            {
                BooleanOperator model = Model as BooleanOperator;
                if (model.Operands.Count == 0) return 0;
                return (model.Operands[0] is BooleanOperator) ? 0 : 1;
            }
        }
        public ICommand ChangeOperatorTypeCommand { get; private set; }
        public ICommand AddComparisonOperatorCommand { get; private set; }
        public ICommand AddBooleanOperatorCommand { get; private set; }
        public ICommand RemoveOperatorCommand { get; private set; }
        private void ChangeOperatorType(string operatorType) { Keyword = operatorType; }
        private void AddComparisonOperator()
        {
            BooleanOperator model = this.Model as BooleanOperator;
            if (model == null) return;
            if (!model.IsLeaf) return;

            ComparisonOperator operand = new ComparisonOperator(model);
            model.AddChild(operand);
            ComparisonOperatorViewModel viewModel = new ComparisonOperatorViewModel(this, operand);
            this.Operands.Add(viewModel);
        }
        private void AddInnerBooleanOperator()
        {
            BooleanOperator model = this.Model as BooleanOperator;
            if (model == null) return;

            if (model.IsLeaf)
            {
                AddOuterBooleanOperator();
            }
            else
            {
                BooleanOperator operand = new BooleanOperator(model) { Keyword = BooleanOperators.OR };
                model.AddChild(operand);
                ComparisonOperator child = new ComparisonOperator(operand);
                operand.AddChild(child);
                BooleanOperatorViewModel viewModel = new BooleanOperatorViewModel(this, operand);
                ComparisonOperatorViewModel childVM = new ComparisonOperatorViewModel(viewModel, child);
                viewModel.Operands.Add(childVM);
                this.Operands.Add(viewModel);
            }
        }
        private void AddOuterBooleanOperator()
        {
            BooleanOperator model = this.Model as BooleanOperator;
            if (model == null) return;

            // 0. Remember the parent of this node
            ISyntaxNode parent = model.Parent;
            ISyntaxNodeViewModel parentVM = this.Parent;
            int index_to_replace = -1;
            if (parent is BooleanOperator)
            {
                index_to_replace = ((BooleanOperator)parent).Operands.IndexOf(model);
                if (index_to_replace == -1)
                {
                    throw new ArgumentOutOfRangeException("Model is broken!");
                }
            }

            // 1. Create new node and it's VM which will substitute this current node
            BooleanOperator substitute = new BooleanOperator(parent) { Keyword = BooleanOperators.OR };
            substitute.AddChild(model);
            BooleanOperatorViewModel substituteVM = new BooleanOperatorViewModel(parentVM, substitute);
            this.Parent = substituteVM;

            // 2. Create new child and it's VM consumed by substitute
            BooleanOperator child = new BooleanOperator(substitute);
            substitute.AddChild(child);
            BooleanOperatorViewModel childVM = new BooleanOperatorViewModel(substituteVM, child);

            // 3. Add new comparison operator and it's VM to new born child
            ComparisonOperator gift = new ComparisonOperator(child);
            child.AddChild(gift);
            ComparisonOperatorViewModel giftVM = new ComparisonOperatorViewModel(childVM, gift);
            childVM.Operands.Add(giftVM);

            // 4. Fill substitute VM with operands
            //substituteVM.Operands.Add(this); TODO: refactoring of logic is needed !
            substituteVM.Operands.Add(childVM);

            // 5. Substitute this current node at parent VM and it's model
            if (parent is BooleanOperator)
            {
                ((BooleanOperator)parent).Operands.RemoveAt(index_to_replace);
                ((BooleanOperator)parent).Operands.Insert(index_to_replace, substitute);
                index_to_replace = ((BooleanOperatorViewModel)parentVM).Operands.IndexOf(this);
                if (index_to_replace > -1)
                {
                    ((BooleanOperatorViewModel)parentVM).Operands.RemoveAt(index_to_replace);
                    ((BooleanOperatorViewModel)parentVM).Operands.Insert(index_to_replace, substituteVM);
                }
            }
            else if (parentVM is OnSyntaxNodeViewModel)
            {
                substituteVM.Parent = parentVM; // TODO: refactoring of logic is needed !
                ((OnSyntaxNodeViewModel)parentVM).Expression = substituteVM;
            }
            else if (parentVM is WhereClauseViewModel)
            {
                substituteVM.Parent = parentVM; // TODO: refactoring of logic is needed !
                ((WhereClauseViewModel)parentVM).Expression = substituteVM;
            }
        }
        public void RemoveChildOperator(ISyntaxNodeViewModel child)
        {
            this.Operands.Remove(child);
        }
        private void RemoveOperator()
        {
            BooleanOperator model = this.Model as BooleanOperator;
            if (model == null) return;

            if (model.IsRoot)
            {
                if (this.Parent is OnSyntaxNodeViewModel)
                {
                    ((OnSyntaxNodeViewModel)this.Parent).Expression = null;
                }
                else if (this.Parent is WhereClauseViewModel)
                {
                    ((WhereClauseViewModel)this.Parent).Expression = null;
                }
            }
            else
            {
                BooleanOperatorViewModel parent = this.Parent as BooleanOperatorViewModel;
                if (parent == null) return;

                BooleanOperator consumer = model.Parent as BooleanOperator;
                if (consumer == null) return;

                if (consumer.Operands.Count == 0) return;

                consumer.Operands.Remove(model);
                parent.RemoveChildOperator(this);

                if (parent.Operands.Count == 1)
                {
                    ISyntaxNodeViewModel orphan = parent.Operands[0];

                    if (parent.Parent is OnSyntaxNodeViewModel)
                    {
                        ((BooleanOperatorViewModel)orphan).Keyword = parent.Keyword;
                        ((OnSyntaxNodeViewModel)parent.Parent).Expression = orphan;
                    }
                    else if (parent.Parent is WhereClauseViewModel)
                    {
                        ((BooleanOperatorViewModel)orphan).Keyword = parent.Keyword;
                        ((WhereClauseViewModel)parent.Parent).Expression = orphan;
                    }
                    else if (parent.Parent is BooleanOperatorViewModel)
                    {
                        orphan.Parent = parent.Parent;
                        orphan.Model.Parent = parent.Parent.Model;

                        int index_to_replace = ((BooleanOperator)consumer.Parent).Operands.IndexOf(consumer);
                        ((BooleanOperator)consumer.Parent).Operands.RemoveAt(index_to_replace);
                        ((BooleanOperator)consumer.Parent).Operands.Insert(index_to_replace, (BooleanOperator)orphan.Model);

                        index_to_replace = ((BooleanOperatorViewModel)parent.Parent).Operands.IndexOf(parent);
                        ((BooleanOperatorViewModel)parent.Parent).Operands.RemoveAt(index_to_replace);
                        ((BooleanOperatorViewModel)parent.Parent).Operands.Insert(index_to_replace, orphan);
                    }
                }
            }
        }
    }
}
