namespace Yu_Gi_Oh_LOTD_LE_Deck_Manager.Services
{
    /// <summary>
    /// Full-buffer snapshot undo/redo for the loaded save file. Every action
    /// that mutates AppContext.State.SaveBytes (rename, change duelist,
    /// copy/swap slot, import .ydk, clear slot, drag-reorder, save a deck
    /// edit, ...) calls Snapshot() immediately BEFORE it mutates the buffer.
    /// Undo/Redo then just swap the live buffer out for a stack entry —
    /// simpler and far less error-prone than tracking per-field diffs across
    /// a dozen different mutation call sites, and the save file is small
    /// enough (a few hundred KB) that cloning it 50 times is trivial memory.
    /// </summary>
    public sealed class UndoManager
    {
        private const int MaxDepth = 50;

        // Newest entry at the end, so trimming the oldest is a cheap
        // RemoveAt(0) instead of rebuilding a Stack<T>.
        private readonly List<byte[]> _undoStack = new();
        private readonly List<byte[]> _redoStack = new();

        public bool CanUndo => _undoStack.Count > 0;
        public bool CanRedo => _redoStack.Count > 0;

        /// <summary>Fired after any push/undo/redo/clear — hosts (MainForm)
        /// use this to enable/disable Undo/Redo buttons.</summary>
        public event Action? StackChanged;

        /// <summary>Call BEFORE mutating AppContext.State.SaveBytes.</summary>
        public void Snapshot()
        {
            byte[]? bytes = AppContext.State.SaveBytes;
            if (bytes is null) return;

            _undoStack.Add((byte[])bytes.Clone());
            if (_undoStack.Count > MaxDepth)
                _undoStack.RemoveAt(0);

            _redoStack.Clear();
            StackChanged?.Invoke();
        }

        public bool Undo()
        {
            byte[]? current = AppContext.State.SaveBytes;
            if (current is null || _undoStack.Count == 0) return false;

            _redoStack.Add((byte[])current.Clone());
            byte[] previous = _undoStack[^1];
            _undoStack.RemoveAt(_undoStack.Count - 1);

            AppContext.State.SaveBytes = previous;
            StackChanged?.Invoke();
            return true;
        }

        public bool Redo()
        {
            byte[]? current = AppContext.State.SaveBytes;
            if (current is null || _redoStack.Count == 0) return false;

            _undoStack.Add((byte[])current.Clone());
            byte[] next = _redoStack[^1];
            _redoStack.RemoveAt(_redoStack.Count - 1);

            AppContext.State.SaveBytes = next;
            StackChanged?.Invoke();
            return true;
        }

        /// <summary>Called when a new save file is opened — the old undo
        /// history no longer applies to the freshly loaded buffer.</summary>
        public void Clear()
        {
            _undoStack.Clear();
            _redoStack.Clear();
            StackChanged?.Invoke();
        }
    }
}
