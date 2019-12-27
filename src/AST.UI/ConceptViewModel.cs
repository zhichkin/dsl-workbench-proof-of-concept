using System.Collections.ObjectModel;

namespace OneCSharp.AST.UI
{
    public interface IConceptViewModel
    {
        //ObservableCollection<LineViewModel> Lines { get; } // TODO: declare interface !!! ???
    }
    public sealed class ConceptViewModel : IConceptViewModel
    {
        private readonly ConceptViewModel _model;
        public ConceptViewModel(ConceptViewModel model)
        {
            _model = model;
        }
        public ObservableCollection<LineViewModel> Lines { get; } = new ObservableCollection<LineViewModel>();
        public void BreakLine(IConceptViewModel item)
        {
            for (int current = 0; current < Lines.Count; current++)
            {
                LineViewModel line = Lines[current];
                int position = line.Items.IndexOf(item);
                if (position == -1 || position == 0) continue; // position == 0 means no empty line allowed
                if (line.Items.Count == 1) return; // no empty line allowed

                LineViewModel newLine = new LineViewModel(this);
                while (position != line.Items.Count)
                {
                    newLine.Items.Add(line.Items[position]);
                    line.Items.RemoveAt(position);
                }
                Lines.Insert(++current, newLine);
            }
            if (item is KeywordViewModel)
            {
                FocusManager.SetFocus((KeywordViewModel)item);
            }
        }
        public void RestoreLine(IConceptViewModel item)
        {
            if (Lines.Count == 0 || Lines.Count == 1) return;

            for (int current = 1; current < Lines.Count; current++)
            {
                LineViewModel line = Lines[current];
                int position = line.Items.IndexOf(item);
                if (position != 0) continue; // only first item can restore line
                
                LineViewModel restoringLine = Lines[--current];
                while (position != line.Items.Count)
                {
                    restoringLine.Items.Add(line.Items[position]);
                    line.Items.RemoveAt(position);
                }
                Lines.RemoveAt(++current);
            }
            if (item is KeywordViewModel)
            {
                FocusManager.SetFocus((KeywordViewModel)item);
            }
        }
        public void FocusLeft(IConceptViewModel item)
        {
            for (int current = 0; current < Lines.Count; current++)
            {
                LineViewModel line = Lines[current];
                int position = line.Items.IndexOf(item);
                if (position == -1) continue;
                if (position == 0) return;

                if (line.Items[position - 1] is KeywordViewModel)
                {
                    FocusManager.SetFocus((KeywordViewModel)line.Items[position - 1]);
                }
            }
        }
        public void FocusRight(IConceptViewModel item)
        {
            for (int current = 0; current < Lines.Count; current++)
            {
                LineViewModel line = Lines[current];
                int position = line.Items.IndexOf(item);
                if (position == -1) continue;
                if (position == line.Items.Count - 1) return;

                if (line.Items[position + 1] is KeywordViewModel)
                {
                    FocusManager.SetFocus((KeywordViewModel)line.Items[position + 1]);
                }
            }
        }
        public void IndentLine(IConceptViewModel item)
        {
            // TODO: don't forget about remove indent command
        }
    }
}