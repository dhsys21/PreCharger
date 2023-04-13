using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PreCharger
{
    public partial class KeysightForm : Form
    {
        #region Delegate - MainForm에서 사용
        public delegate void delegateReport_SendCommand(int stageno, string command);
        public event delegateReport_SendCommand OnSendCommand = null;
        protected void RaiseOnSendCommand(int stageno, string command)
        {
            if (OnSendCommand != null)
            {
                OnSendCommand(stageno, command);
            }
        }
        #endregion
        public KeysightForm()
        {
            InitializeComponent();
        }

        
        #region Keysight Command
        private void SetCommand(int nCommandIndex, string command)
        {
            string commandtype = string.Empty;
            string commandtext = string.Empty;
            try
            {
                commandtype = command.Split('.')[0].Trim();
                commandtext = command.Split('.')[1].Trim();
                if (Convert.ToInt16(commandtype) < 20)
                    tbCommand.Text = commandtext;
                else 
                {
                    switch (Convert.ToInt16(commandtype))
                    {
                        case 21: // SET
                            tbCommand.Text = commandtext;
                            break;
                        case 22: // CHARGING
                            break;
                        case 23: // DISCHARGING
                            break;
                        case 24: // ABORT
                            break;
                        default:
                            break;
                    }
                }
            }
            catch(Exception ex)
            {
                tbMsg.Text = "SetCommand Error : " + ex.ToString();
                Console.WriteLine(ex.ToString());
            }
        }
        private void SENDCOMMAND()
        {
            string CMD = tbCommand.Text;
            int stageno = cbKeysigt.SelectedIndex;
            RaiseOnSendCommand(stageno, CMD);

        }
        private void SET()
        {

        }
        private void CHARGING()
        {

        }
        private void DISCHARGING()
        {

        }
        private void ABORT()
        {

        }
        #endregion

        #region Event
        private void btnSendCommand_Click(object sender, EventArgs e)
        {
            SENDCOMMAND();
        }
        private void lblCmd_Click(object sender, EventArgs e)
        {
            Label lbl = (Label)sender;
            int nCommandIndex = int.Parse(lbl.Tag.ToString());
            SetCommand(nCommandIndex, lbl.Text);
        }
        #endregion

        public void SetResult(string results)
        {
            if (tbMsg.InvokeRequired)
            {
                // 작업쓰레드인 경우
                tbMsg.BeginInvoke(new Action(() => tbMsg.AppendText(results + Environment.NewLine + Environment.NewLine)));
            }
            else
            {
                // UI 쓰레드인 경우
                tbMsg.AppendText(results + Environment.NewLine + Environment.NewLine);
            }
        }

        
    }
}
