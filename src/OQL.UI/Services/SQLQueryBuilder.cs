using OneCSharp.Metadata;
using OneCSharp.OQL.Model;
using System.Linq;
using System.Text;

namespace OneCSharp.OQL.UI.Services
{
    public interface IQueryBuilder
    {
        string Build(Procedure procedure);
    }
    public sealed class SQLServerQueryBuilder : IQueryBuilder
    {
        private StringBuilder sql;
        public string Build(Procedure procedure)
        {
            sql = new StringBuilder();
            foreach (ISyntaxNode syntaxNode in procedure.Statements)
            {
                VisitSyntaxNode(syntaxNode);
            }
            return sql.ToString();
        }
        private void VisitSyntaxNode(ISyntaxNode syntaxNode)
        {
            if (syntaxNode is Parameter)
            {
                VisitParameter((Parameter)syntaxNode);
            }
            else if (syntaxNode is SelectStatement)
            {
                VisitSelectStatement((SelectStatement)syntaxNode);
            }
            else if (syntaxNode is JoinOperator)
            {
                VisitJoinOperator((JoinOperator)syntaxNode);
            }
            else if (syntaxNode is HintSyntaxNode)
            {
                VisitHintSyntaxNode((HintSyntaxNode)syntaxNode);
            }
            else if (syntaxNode is AliasSyntaxNode)
            {
                VisitAliasSyntaxNode((AliasSyntaxNode)syntaxNode);
            }
            else if (syntaxNode is TableObject)
            {
                VisitTableObject((TableObject)syntaxNode);
            }
            else if (syntaxNode is PropertyObject)
            {
                VisitPropertyObject((PropertyObject)syntaxNode);
            }
            else if (syntaxNode is PropertyReference)
            {
                VisitPropertyReference((PropertyReference)syntaxNode);
            }
            else if (syntaxNode is BooleanOperator)
            {
                VisitBooleanOperator((BooleanOperator)syntaxNode);
            }
            else if (syntaxNode is ComparisonOperator)
            {
                VisitComparisonOperator((ComparisonOperator)syntaxNode);
            }
        }
        private void VisitParameter(Parameter syntaxNode)
        {
            sql.Append($"@{syntaxNode.Name}");
        }
        private void VisitJoinOperator(JoinOperator syntaxNode)
        {
            sql.AppendLine($"\t{syntaxNode.JoinType} {syntaxNode.Keyword}");
            VisitSyntaxNode(syntaxNode.Expression);
            sql.AppendLine($"\tON");
            VisitBooleanFunction(syntaxNode.ON.Expression);
        }
        private void VisitHintSyntaxNode(HintSyntaxNode syntaxNode)
        {
            VisitSyntaxNode(syntaxNode.Expression);
            if (syntaxNode.Parent is AliasSyntaxNode)
            {
                sql.Append($" AS [{((AliasSyntaxNode)syntaxNode.Parent).Alias}]");
            }
            sql.Append($" WITH({syntaxNode.HintType})");
        }
        private void VisitAliasSyntaxNode(AliasSyntaxNode syntaxNode)
        {
            if (syntaxNode.Expression is HintSyntaxNode)
            {
                VisitHintSyntaxNode((HintSyntaxNode)syntaxNode.Expression);
            }
            else
            {
                VisitSyntaxNode(syntaxNode.Expression);
                sql.Append($" AS [{syntaxNode.Alias}]");
            }
        }
        private void VisitTableObject(TableObject syntaxNode)
        {
            sql.AppendLine($"\t{syntaxNode.Table.TableName}");
        }
        private void VisitPropertyObject(PropertyObject syntaxNode)
        {
            sql.Append($"{syntaxNode.Name}");
        }
        private void VisitBooleanFunction(ISyntaxNode syntaxNode)
        {
            if (syntaxNode is BooleanOperator)
            {
                VisitBooleanOperator((BooleanOperator)syntaxNode);
            }
            else if (syntaxNode is ComparisonOperator)
            {
                VisitComparisonOperator((ComparisonOperator)syntaxNode);
            }
        }
        private void VisitBooleanOperator(BooleanOperator syntaxNode)
        {
            int counter = 0;

            sql.AppendLine("\t(");
            foreach (ISyntaxNode function in syntaxNode.Operands)
            {
                sql.AppendLine("\t");
                if (counter > 0)
                {
                    sql.Append($"{syntaxNode.Keyword} ");
                }
                VisitBooleanFunction(function);
                counter++;
            }
            sql.AppendLine("\t)");
        }
        private void VisitComparisonOperator(ComparisonOperator syntaxNode)
        {
            if (syntaxNode.IsRoot) sql.AppendLine("\t(");
            VisitSyntaxNode(syntaxNode.LeftExpression);
            sql.Append($" {syntaxNode.Literal} ");
            VisitSyntaxNode(syntaxNode.RightExpression);
            if (syntaxNode.IsRoot) sql.Append(")");
        }
        private void VisitSelectStatement(SelectStatement syntaxNode)
        {
            sql.Append("SELECT");
            int currentOrdinal = 0;
            foreach (ISyntaxNode property in syntaxNode.SELECT)
            {
                //VisitPropertyReference(property, ref currentOrdinal);
                if (currentOrdinal > 0)
                {
                    sql.AppendLine(", ");
                }
                VisitSyntaxNode(property);
                currentOrdinal++;
            }

            sql.AppendLine("FROM");
            foreach (ISyntaxNode table in syntaxNode.FROM)
            {
                VisitSyntaxNode(table);
            }

            if (syntaxNode.WHERE != null && syntaxNode.WHERE.Expression != null)
            {
                sql.AppendLine("WHERE");
                VisitBooleanFunction(syntaxNode.WHERE.Expression);
            }
        }
        private void VisitPropertyReference(PropertyReference syntaxNode)
        {
            string aliasTable = string.Empty;
            AliasSyntaxNode alias = syntaxNode.TableSource as AliasSyntaxNode;
            if (alias == null) { alias = ((JoinOperator)syntaxNode.TableSource).Expression as AliasSyntaxNode; }
            if (alias != null) aliasTable = alias.Alias;

            DbProperty property = ((PropertyObject)syntaxNode.PropertySource).Property;

            DbField field = property.Fields.Where(f => f.Purpose == DbFieldPurpose.Value).FirstOrDefault();
            if (field == null)
            {
                field = property.Fields.Where(f => f.Purpose == DbFieldPurpose.Object).FirstOrDefault();
            }
            if (field == null)
            {
                sql.Append($"[{aliasTable}].[{property.Name}]");
            }
            else
            {
                sql.Append($"[{aliasTable}].[{field.Name}]");
            }
        }
        //private void VisitPropertyReference(PropertyReference syntaxNode, ref int currentOrdinal)
        //{
        //    PropertyReference property = syntaxNode.syntaxNode as PropertyReference;
        //    if (property == null) return;

        //    if (currentOrdinal == 0) { sql.Append("\n"); }

        //    PropertyReferenceManager manager = new PropertyReferenceManager(property);
        //    manager.Prepare(ref currentOrdinal);
        //    propertyManagers.Add(syntaxNode.Alias, manager);

        //    sql.Append($"\t{manager.ToSQL()}");
        //}
    }
}
