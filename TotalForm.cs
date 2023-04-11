using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PreCharger.Common;


namespace PreCharger
{
	
    public partial class TotalForm : Form
    {
		//* keysight
		// Constants
		int NUM_CHANNELS = 32;
		double remain = 0.0;
		double timeInterval = 0.0;
		double duration = 0.0;
		bool TraceUpdateOn = true;
		string returnValue = @"";

		private Util util;
		private EquipProcess _EQProcess = null;
		
		public int stage;

		#region Delegate
		public delegate void delegateReport_ViewMeasureInfo(int stageno, bool bManualMode);
		public event delegateReport_ViewMeasureInfo OnViewMeasureInfo = null;
		protected void RaiseOnViewMeasureInfo(int stageno, bool bManualMode)
		{
			if (OnViewMeasureInfo != null)
			{
				OnViewMeasureInfo(stageno, bManualMode);
			}
		}
		#endregion

		private static TotalForm[] totalForm = new TotalForm[_Constant.frmCount];
		public static TotalForm GetInstance(int nIndex)
		{
			if (totalForm[nIndex] == null) totalForm[nIndex] = new TotalForm();
			return totalForm[nIndex];
		}
		public TotalForm()
        {
            InitializeComponent();
			//totalForm = this;
			util = new Util();
			_EQProcess = EquipProcess.GetInstance();
			//_measureinfoform = FormMeasureInfo.GetInstance();
			//panel = new DoubleBufferedPanel[256];

			gridView.DoubleBuffered(true);

			BaseForm.frmMain.OnLabelTrayId += _LabelTrayId;
		}

        private void TatalForm_Load(object sender, EventArgs e)
        {
            lblTitle.Text = "STAGE " + (stage + 1).ToString();

			makeGridView();
			initGridView();
			//MakePanel();
		}

		public void initPanel()
        {
			for (int nIndex = 0; nIndex < 256; nIndex++)
			{
				//panel[nIndex].BackColor = Color.SkyBlue;
			}
		}

        #region PLC Status
        public void OnTrayId(string trayid)
        {
            tbTrayId.Text = trayid;
        }

		public void OnTrayIN(int iValue)
        {
			SetLabelColor(lblTrayIn, iValue);
        }

		public void OnProbeOpen(int iValue)
        {
			SetLabelColor(lblProbeOpen, iValue);
        }

		public void OnProbeClose(int iValue)
        {
			SetLabelColor(lblProbeClose, iValue);
        }

		public void OnAutoManual(int iValue)
        {
			SetLabelColor(lblPLCAuto, iValue);
        }

		public void OnPLCError(int iValue)
        {
			SetLabelColor(lblPLCError, iValue);
        }
		#endregion
		public void SetChargingTime(int nSeconds)
		{
			lblTestTime.Text = nSeconds.ToString();
		}
		public void SetLabelColor(Label lbl, int iValue)
        {
			if (iValue == 1)
			{
				if(lbl.BackColor != Color.LimeGreen)
					lbl.BackColor = Color.LimeGreen;
			}
			else
			{
				if(lbl.BackColor != Color.Red)
					lbl.BackColor = Color.Red;
			}
        }

		public void SetConnectInfo(int nState)
		{
			if (nState == 1)
			{
				if(lblConnInfo.BackColor != Color.LightSkyBlue)
                {
					lblConnInfo.BackColor = Color.LightSkyBlue;
					lblConnInfo.Text = "PreCharger is connected";
				}
			}
			else if (nState == 0)
			{
				if(lblConnInfo.BackColor != Color.Red)
                {
					lblConnInfo.BackColor = Color.Red;
					lblConnInfo.Text = "PreCharger is disconnected";
				}
			}
		}

		public void SetTrayId(string trayid)
		{
			// nForm lblStep.text = GetPreChargerStatus(_sStageStatus)
			// nForm lblTestTime.text = int.Parse(_iTestTime / 60).tostring() + " : " + int.parse(_iTestTime % 60).tostring();

			try
			{
				this.Invoke(new MethodInvoker(delegate ()
				{
					tbTrayId.Text = trayid;
				}));
			}
			catch (Exception ex)
			{
				//_Logger.Log(Level.Exception, "_tmrSystem_Elapsed : " + ex.ToString());
			}
		}

		private void _LabelTrayId(int nIndex, string trayid)
		{
			try
			{
				this.Invoke(new MethodInvoker(delegate ()
				{
					if (this.stage == nIndex)
						tbTrayId.Text = trayid;
				}));
			}
			catch (Exception ex)
			{
				//_Logger.Log(Level.Exception, "_tmrSystem_Elapsed : " + ex.ToString());
			}
			//if(this.stage == nIndex)
			//	tbTrayId.Text = trayid;

		}

		public void SetTestTimeInfo(string sStep, string sTesttime)
        {
			// nForm lblStep.text = GetPreChargerStatus(_sStageStatus)
			// nForm lblTestTime.text = int.Parse(_iTestTime / 60).tostring() + " : " + int.parse(_iTestTime % 60).tostring();

			try
			{
				this.Invoke(new MethodInvoker(delegate ()
				{
					lblStep.Text = sStep;
					lblTestTime.Text = sTesttime;
				}));
			}
			catch (Exception ex)
			{
				//_Logger.Log(Level.Exception, "_tmrSystem_Elapsed : " + ex.ToString());
			}
		}

		public void SetDisplayStatus(string sStatus)
        {
			btnManualTest.Visible = false;
			switch(sStatus)
            {
				case "NoAnswer":
					pictureStatus.Image = Properties.Resources.Stage_NoAnswer;
					break;
				case "Vacancy":
					pictureStatus.Image = Properties.Resources.Stage_Vacancy;
					break;
				case "Ready":
					pictureStatus.Image = Properties.Resources.Stage_Ready;
					break;
				case "TrayIn":
					pictureStatus.Image = Properties.Resources.Stage_TrayIn;
					break;
				case "Run":
					pictureStatus.Image = Properties.Resources.Stage_Run;
					break;
				case "End":
					pictureStatus.Image = Properties.Resources.Stage_End;
					break;
				case "TrayOut":
					pictureStatus.Image = Properties.Resources.Stage_TrayOut;
					break;
				case "Manual":
					pictureStatus.Image = Properties.Resources.Stage_Manual;
					btnManualTest.Visible = true;
					break;
				case "EmergencyStop":
					pictureStatus.Image = Properties.Resources.Stage_EmergencyStop;
					break;
				case "Reset":
					pictureStatus.Image = Properties.Resources.Stage_Reset;
					break;
				default:
					break;
            }
			
        }

		private void SetColorToGridView(DataGridView dgv, int nIndex, Color clr)
        {
			//int rowIndex = nIndex / 16;
			//int colIndex = nIndex % 16;

			int rowIndex, colIndex;
			util.ChangeMapToGridView(nIndex, out rowIndex, out colIndex);
            dgv.Rows[rowIndex].Cells[colIndex].Style.BackColor = clr;

		}

		private Color GetColorFromGridView(DataGridView dgv, int nIndex)
        {
			//int rowIndex = nIndex / 16;
			//int colIndex = nIndex % 16;

			int rowIndex, colIndex;
			util.ChangeMapToGridView(nIndex, out rowIndex, out colIndex);
			return dgv.Rows[rowIndex].Cells[colIndex].Style.BackColor;
        }

        public void DisplayChannelInfo(CPrechargerData CPreData, enumEquipStatus enumequipstatus)
        {
            try
            {
                for (int nIndex = 0; nIndex < 256; nIndex++)
                {
					enumChannelStatus eChannelStatus = CPreData.ChannelStatus[nIndex];
					bool iCellExist = CPreData.CELL[nIndex];
					double dVolt = 0;
					double.TryParse(CPreData.VOLT[nIndex], out dVolt);
					Color clrChannelColor = CPreData.CHANNELCOLOR[nIndex];

					if (enumequipstatus == enumEquipStatus.StepTrayIn && iCellExist == false) 
						SetColorToGridView(gridView, nIndex, _Constant.ColorNoCell);

					//if (CPreData.AMF == true && panel[nIndex].BackColor == _Constant.ColorCharging)
					if (CPreData.AMF == true && GetColorFromGridView(gridView, nIndex) == _Constant.ColorCharging)
					{
						//SetColorToPanel(pnl, _Constant.ColorFinish);
						SetColorToGridView(gridView, nIndex, _Constant.ColorFinish);
					}
                    else if(CPreData.AMS == true && CPreData.PrechargerStatus == enumPrechargerStatus.Charging)
                    {
						//SetColorToPanel(pnl, clrChannelColor);
						SetColorToGridView(gridView, nIndex, clrChannelColor);
					}
                }
            }
			catch(Exception ex)
            {

            }
        }

		private void makeGridView()
		{
			#region 행/열 제목, 갯수
			for (int nIndex = 0; nIndex < 16; nIndex++)
			{
				gridView.Columns.Add("CH" + nIndex.ToString("D2"), (nIndex + 1).ToString("D2"));
				gridView.Columns[nIndex].Width = 28;
				
			}

			gridView.Rows.Clear();
			gridView.Rows.Add(16);
			for (int i = 0; i < 16; i++)
			{
				gridView.Rows[i].Height = 14;
				/// Divider line
				if(i % 2 == 1)
					gridView.Rows[i].DividerHeight = 2;
			}

			int rowIndex, colIndex;
			for (int i = 0; i < 256; i++)
            {
				//gridView.Rows[16 - 1 - i / 16].Cells[i % 16].Tag = i;
				util.ChangeMapToGridView(i, _Constant.StartPosition, out rowIndex, out colIndex);
				gridView.Rows[rowIndex].Cells[colIndex].Tag = i;
			}

			gridView.RowHeadersVisible = false;
			gridView.ColumnHeadersVisible = false;
			gridView.RowsDefaultCellStyle.BackColor = _Constant.ColorReady;
			gridView.AlternatingRowsDefaultCellStyle.BackColor = _Constant.ColorReady;
			gridView.ClearSelection();
			#endregion
		}

		private void gridView_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
		{
			int rowIndex, colIndex, tag;
			try
            {
				if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
				{
					//DataGridViewCell cell = this.gridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
					//cell.ToolTipText = (e.RowIndex * 16 + e.ColumnIndex + 1)
					//    + " (" + (e.RowIndex + 1).ToString() + " - " + (e.ColumnIndex + 1).ToString() + ")";
					var tagObj = gridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag;
					tag = 0;
					if (tagObj != null)
						tag = int.Parse(tagObj.ToString());
					//tag = int.Parse(gridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Tag.ToString());
                    DataGridViewCell cell = this.gridView.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    cell.ToolTipText = (tag + 1)
                        + " (" + (tag / 16 + 1).ToString() + " - " + (tag % 16 + 1).ToString() + ")";
                }
			}
			catch(Exception ex) { }
			
			
		}

		public void initGridView()
		{
			gridView.RowsDefaultCellStyle.BackColor = _Constant.ColorReady;
			gridView.ClearSelection();

			for(int nIndex = 0; nIndex < 256; nIndex++)
            {
				gridView.Rows[nIndex / 16].Cells[nIndex % 16].Style.BackColor = _Constant.ColorReady;
            }
		}

		private void SetColorToPanel(Panel pnl, Color clr)
        {
			pnl.BackColor = clr;
        }

		private void MakePanel()
        {
			DoubleBufferedPanel pBase2 = new DoubleBufferedPanel();
			pBase2.DoubleBuffered = true;
			pBase2.Width = 451;
			pBase2.Height = 239;

			string type = "1";
			
			int nx, ny, nw, nh;

			if (type == "1" || type == "2")
			{
				nh = 13;
				nw = 27;

                nx = 2;
                ny = 225;

				for (int index = 0; index < 256;)
				{
					//panel[index] = new DoubleBufferedPanel();
					//panel[index].DoubleBuffered = true;
     //               pBase2.Controls.Add(panel[index]);
     //               panel[index].Left = nx;
					//panel[index].Top = ny;
					//panel[index].Width = nw;
					//panel[index].Height = nh;

					//panel[index].BackColor = Color.SkyBlue;//"$00FFE8D9";
					//panel[index].Tag = index;

					index += 1;
					nx = nx + nw + 1;
					if (index % 16 == 0)
					{
						ny = ny - nh - 1;
						nx = 2;
						if ((index / 16) % 2 == 0) ny -= 2;
					}
				}
			}
			//else if (type == "3" || type == "4")
			//{
			//	nh = 13;
			//	nw = 27;
			//	nx = 421;
			//	//nx = 2;
			//	ny = 225;


			//	for (int index = 0; index < 256;)
			//	{
			//		panel[index] = new TPanel(this);
			//		panel[index]->Parent = pBase;
			//		panel[index]->Left = nx;
			//		panel[index]->Top = ny;
			//		panel[index]->Width = nw;
			//		panel[index]->Height = nh;

			//		panel[index]->Color = pnormal1->Color;
			//		panel[index]->ParentBackground = false;

			//		panel[index]->BevelInner = bvNone;
			//		panel[index]->BevelKind = bkNone;
			//		panel[index]->BevelOuter = bvNone;
			//		panel[index]->Tag = index;
			//		//		panel[index]->Caption = index;

			//		panel[index]->Hint = IntToStr(index + 1) + " (" + IntToStr((index + 16) / 16) + "-" + IntToStr((index % 16) + 1) + ")";
			//		panel[index]->ShowHint = true;

			//		panel[index]->OnMouseEnter = ChInfoMouseEnter;
			//		panel[index]->OnMouseLeave = ChInfoMouseLeave;

			//		index += 1;
			//		nx = nx - nw - 1;
			//		if (index % 16 == 0)
			//		{
			//			ny = ny - nh - 1;
			//			nx = 421;
			//			if ((index / 16) % 2 == 0) ny -= 2;
			//		}
			//	}
			//}

			pBase.Controls.Add(pBase2);
		}

        #region Precharger Command
        private void CmdAutoManual(bool bMode)
        {
            if (bMode == true) // Auto Mode
            {
				BaseForm.frmMain.CmdAuto(this.stage);
				SetDisplayStatus("Vacancy");
            }
            else              // Manual Mode
            {
				BaseForm.frmMain.CmdManual(this.stage);
				SetDisplayStatus("Manual");
            }

            BaseForm.frmMain.CmdStop(this.stage);

			/// init formtotal panel, measureinfoform
			//BaseForm.frmMain.InitDisplayInfo(this.stage);
			EquipProcess.equipprocess.InitDisplayInfo(this.stage);
		}

		public void CmdSet()
        {
			_EQProcess.SetPrecharger(this.stage, tbVoltage.Text, tbCurrent.Text, tbTime.Text);
		}
        #endregion

        private void TrayOut()
        {
			BaseForm.frmMain.SetTrayOut(this.stage);
        }

		private void ViewMeasureInfo(bool bValue)
        {
			//_measureinfoform.SetStage(this.stage);
			//_measureinfoform.Visible = true; ;
			//_measureinfoform.BringToFront();
			//_measureinfoform.SetManualMode(bValue);
		}

        private void btnViewMeasureInfo_Click(object sender, EventArgs e)
        {
			//ViewMeasureInfo(false);
			RaiseOnViewMeasureInfo(this.stage, false);

		}

        private void btnTrayOut_Click(object sender, EventArgs e)
        {
			TrayOut();
        }

        private void btnManualMode_Click(object sender, EventArgs e)
        {
			CmdAutoManual(false);
        }

        private void btnAutoMode_Click(object sender, EventArgs e)
        {
			CmdAutoManual(true);
        }

        private void btnManualTest_Click(object sender, EventArgs e)
        {
			RaiseOnViewMeasureInfo(this.stage, true);
			//ViewMeasureInfo(true);
		}

        private void button3_Click(object sender, EventArgs e)
        {
			
        }

        private void btnInitialization_Click(object sender, EventArgs e)
        {
			EquipProcess.equipprocess.Initialization(this.stage);

        }

        private void lblTrayOut_Click(object sender, EventArgs e)
        {
			BaseForm.frmMain.SetPCError(this.stage, 0);
        }

        private void button1_Click(object sender, EventArgs e)
        {

		}
    }

    public class DoubleBufferedPanel : Panel
	{
		[DefaultValue(true)]
		public new bool DoubleBuffered
		{
			get
			{
				return base.DoubleBuffered;
			}
			set
			{
				base.DoubleBuffered = value;
			}
		}
	}
	
}
