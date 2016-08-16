using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DxLibDLL;

namespace csFormDX
{
    public partial class Form1 : Form
	{
		public Form1()
        {
            InitializeComponent();

            DX.SetUserWindow(screen.Handle);//描画先をに設定
            DX.SetUseDirectInputFlag(DX.FALSE);// DirectInput初期化を行わない
            //DX.SetFullSceneAntiAliasingMode(4, 2);//フルスクリーンアンチエイリアス
            DX.DxLib_Init();                //初期化
            //DX.SetGraphMode(pictureBox1.Size.Width, pictureBox1.Size.Height, 32);
            DX.SetGraphMode(1280, 960, 32);
            DX.SetDrawMode(DX.DX_DRAWMODE_BILINEAR);
            DX.SetWindowSize(screen.Size.Width, screen.Size.Height);
            DX.SetCameraNearFar(1f, 1000.0f);
            DX.SetUseZBuffer3D(DX.TRUE);
            DX.SetWriteZBuffer3D(DX.TRUE);
            DX.MV1SetLoadModelUsePhysicsMode(DX.DX_LOADMODEL_PHYSICS_REALTIME);
            //DX.MV1SetLoadModelPhysicsWorldGravity(-500);
            DX.MakeKeyInput(1, 0, 0, 0);
            //ライト追加
            /*int LightHandle = DX.CreatePointLightHandle(
                            DX.VGet(0, 100.0f, 0),
                            2.0f,
                            0.99f,
                            0.99f,
                            0.99f);*/
        }

        private void pictureBox1_DoubleClick(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Maximized)
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
            }
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
		{
			DX.SetWindowSize(screen.Size.Width, screen.Size.Height);
            //DX.SetGraphMode(pictureBox1.Size.Width, pictureBox1.Size.Height, 32);
        }
    }
}
