using System;
using System.Windows.Forms;
using YouMask.Properties;

namespace YouMask
{
    public partial class FrmMain : Form
    {
        public const uint B001 = 1;
        public const uint B010 = 1 << 1;
        public const uint B100 = 1 << 2;

        private bool _ignoreTextChange = false;
        private bool _ignoreCheckChange = false;

        private readonly Settings _s = Settings.Default;

        public FrmMain()
        {
            InitializeComponent();

            tbMask.Text = _s.LastMask.ToString("000");

            MaskFromString();
        }

        private void Mask_CheckedChanged(object sender, EventArgs e)
        {
            MaskFromCheckBoxes();
        }

        private void MaskFromString()
        {
            if (_ignoreTextChange)
            {
                return;
            }

            _ignoreCheckChange = true;

            uint mask;
            if (!uint.TryParse(tbMask.Text, out mask))
            {
                return;
            }

            uint uMask = mask / 100,
                 gMask = (mask % 100) / 10,
                 oMask = (mask % 10);

            tbNotMask.Text = ToNotMask(uMask, gMask, oMask).ToString("000");

            cbUR.Checked = (uMask & B100) != 0;
            cbUW.Checked = (uMask & B010) != 0;
            cbUX.Checked = (uMask & B001) != 0;

            cbGR.Checked = (gMask & B100) != 0;
            cbGW.Checked = (gMask & B010) != 0;
            cbGX.Checked = (gMask & B001) != 0;

            cbOR.Checked = (oMask & B100) != 0;
            cbOW.Checked = (oMask & B010) != 0;
            cbOX.Checked = (oMask & B001) != 0;

            _ignoreCheckChange = false;
        }

        private void MaskFromCheckBoxes()
        {
            if (_ignoreCheckChange)
            {
                return;
            }

            _ignoreTextChange = true;

            uint uMask = 0,
                 gMask = 0,
                 oMask = 0;

            // U
            if (cbUR.Checked)
            {
                uMask |= B100;
            }

            if (cbUW.Checked)
            {
                uMask |= B010;
            }

            if (cbUX.Checked)
            {
                uMask |= B001;
            }

            // G
            if (cbGR.Checked)
            {
                gMask |= B100;
            }

            if (cbGW.Checked)
            {
                gMask |= B010;
            }

            if (cbGX.Checked)
            {
                gMask |= B001;
            }

            // O
            if (cbOR.Checked)
            {
                oMask |= B100;
            }

            if (cbOW.Checked)
            {
                oMask |= B010;
            }

            if (cbOX.Checked)
            {
                oMask |= B001;
            }

            var mask = (uMask * 100) + (gMask * 10) + oMask;
            tbMask.Text = mask.ToString("000");
            tbNotMask.Text = ToNotMask(uMask, gMask, oMask).ToString("000");

            _ignoreTextChange = false;
        }

        private static uint ToNotMask(uint uMask, uint gMask, uint oMask)
        {
            return ((uMask ^ 7) * 100) + ((gMask ^ 7) * 10) + (oMask ^ 7);
        }

        private void tbMask_TextChanged(object sender, EventArgs e)
        {
            MaskFromString();
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            int currentMask;
            if (!int.TryParse(tbMask.Text, out currentMask))
            {
                return;
            }
            _s.LastMask = currentMask;
            _s.Save();
        }
    }
}