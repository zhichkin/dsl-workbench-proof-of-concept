# DSL workbench
Концепция платформы (proof of concept) разработки и использования модульных DSL.

Грамматика DSL языка (концепции) задаётся классом на языке C#.

```C#
public sealed class SelectConcept : SyntaxNode
    {
        public SelectConcept()
        {
            FROM = new FromConcept() { Parent = this };
        }
        public Optional<bool> IsDistinct { get; } = new Optional<bool>();
        [TypeConstraint(typeof(int), typeof(ParameterConcept), typeof(VariableConcept))]
        public Optional<object> TopExpression { get; } = new Optional<object>();
        public Optional<List<SelectExpression>> Expressions { get; } = new Optional<List<SelectExpression>>();
        public FromConcept FROM { get; private set; }
        public Optional<WhereConcept> WHERE { get; } = new Optional<WhereConcept>();
    }
```

Шаблон отображения концепций DSL задаётся при помощи Fluent API на C#.

```C#
public sealed class SelectConceptLayout : ConceptLayout<SelectConcept>
    {
        public override ISyntaxNodeViewModel Layout(SelectConcept concept)
        {
            return (new ConceptNodeViewModel(null, concept))
                .Keyword("SELECT")
                .Property(nameof(concept.IsDistinct))
                    .Keyword("DISTINCT")
                .Property(nameof(concept.TopExpression))
                    .Keyword("TOP")
                    .Literal("(")
                    .Selector()
                    .Literal(")")
                .Repeatable().Bind(nameof(concept.Expressions))
                .Concept().Bind(nameof(concept.FROM))
                .Concept().Bind(nameof(concept.WHERE));
        }
    }
```

Редактор кода работает по метаданным класса грамматики, изменяя ViewModel шаблона, используя Reflection.

Демо видео здесь: https://www.youtube.com/watch?v=3SML4jSCL14

Языки модульные, то есть можно использовать в редакторе 2 и более подключенных языков DSL одновременно.
Всё работает аналогично JetBrains MPS. Отличаются только способы создания DSL языков и их редактирования.

