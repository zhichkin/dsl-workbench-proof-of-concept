using Microsoft.VisualStudio.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OQL
{
    public sealed class TextViewController
    {
        private ITextBuffer _buffer;
        private List<ITrackingSpan> _trackingSpans = new List<ITrackingSpan>();
        public TextViewController(ITextBuffer buffer)
        {
            _buffer = buffer;
            BuildCodeTree();
        }
        private void BuildCodeTree()
        {
            var snapshot = _buffer.CurrentSnapshot;
            Span span = new Span(0, 6);
            var trackingSpan = snapshot.CreateTrackingSpan(span, SpanTrackingMode.EdgeExclusive);
            _trackingSpans.Add(trackingSpan);
            
        }
        public ITrackingSpan GetTrackingSpan(SnapshotPoint point)
        {
            var currentSnapshot = _buffer.CurrentSnapshot;
            foreach (var ts in _trackingSpans)
            {
                var currentSpan = ts.GetSpan(currentSnapshot);
                if (currentSpan.Contains(point))
                {
                    return ts;
                }
            }
            return null;
        }
    }
}
