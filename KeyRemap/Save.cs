using System.Collections.Generic;
namespace KeyRemap
{
    public class Save
    {
        public int delay { get; set; }
        public List<Keymap> keymaps { get; set; }

        public Save(int delay, List<Keymap> keymaps)
        {
            this.delay = delay;
            this.keymaps = keymaps;
        }
    }
}
