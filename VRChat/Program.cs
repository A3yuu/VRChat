using System;
using System.Windows.Forms;
using DxLibDLL;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.FaceTracking;


namespace csFormDX
{
    static class Program
	{
		//番号からジョイント
		public static JointType[] jointNum = new JointType[]{
			JointType.Spine,
			JointType.Spine,
			JointType.ShoulderCenter,
			JointType.Head,
			JointType.ShoulderLeft,
			JointType.ElbowLeft,
			JointType.WristLeft,
			JointType.HandLeft,
			JointType.ShoulderRight,
			JointType.ElbowRight,
			JointType.WristRight,
			JointType.HandRight,
			JointType.Spine,
			JointType.HipLeft,
			JointType.KneeLeft,
			JointType.AnkleLeft,
			JointType.FootLeft,
			JointType.HipRight,
			JointType.KneeRight,
			JointType.AnkleRight,
			JointType.FootRight,
		};
		//キー操作を取得
		static int GetHitKeyStateAll_2(ref int[] GetHitKeyStateAll_InputKey)
		{
			byte[] GetHitKeyStateAll_Key = new byte[256];
			DX.GetHitKeyStateAll(out GetHitKeyStateAll_Key[0]);
			for (int i = 0; i < 256; i++)
			{
				if (GetHitKeyStateAll_Key[i] == 1)
				{
					if (GetHitKeyStateAll_InputKey[i] < 0) GetHitKeyStateAll_InputKey[i] = 0;
					GetHitKeyStateAll_InputKey[i]++;
				}
				else
				{
					if (GetHitKeyStateAll_InputKey[i] > 0) GetHitKeyStateAll_InputKey[i] = 0;
					GetHitKeyStateAll_InputKey[i]--;
				}
			}
			return 0;
		}
		//Kinect2DxLib
		static DX.VECTOR[] skeletonKinect2DXLib(Skeleton skeletonData)
		{
			DX.VECTOR[] skeletonDXLib = new DX.VECTOR[jointNum.Length];
			//ボーン飛び検知
			if (skeletonData != null)
			{
				for (int i = 0; i < (int)jointNum.Length; i++)
				{
					skeletonDXLib[i].x = -skeletonData.Joints[jointNum[i]].Position.X;
					skeletonDXLib[i].y = skeletonData.Joints[jointNum[i]].Position.Y;
					skeletonDXLib[i].z = skeletonData.Joints[jointNum[i]].Position.Z;
				}
			}
			return skeletonDXLib;
		}
		//人表示
		public enum FRAME
		{
			CENTER = 0,
			TRS,
			NECK,
			HEAD,
			SHO_L,
			ELB_L,
			RST_L,
			HND_L,
			SHO_R,
			ELB_R,
			RST_R,
			HND_R,
			TRS_UND,
			LEG_L,
			KNE_L,
			ANC_L,
			FOT_L,
			LEG_R,
			KNE_R,
			ANC_R,
			FOT_R,
			MAX
		}
		public static int DrawBone(DX.VECTOR[] BP_Vector)
		{
			const float sizeD = 0.25f;
			DX.DrawSphere3D(BP_Vector[(int)FRAME.CENTER], sizeD, 10, DX.GetColor(255, 0, 255), DX.GetColor(255, 0, 255), 1);
			DX.DrawSphere3D(BP_Vector[(int)FRAME.HEAD], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.HEAD], BP_Vector[(int)FRAME.NECK], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.NECK], BP_Vector[(int)FRAME.SHO_L], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.SHO_L], BP_Vector[(int)FRAME.ELB_L], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.ELB_L], BP_Vector[(int)FRAME.RST_L], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.RST_L], BP_Vector[(int)FRAME.HND_L], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.NECK], BP_Vector[(int)FRAME.SHO_R], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.SHO_R], BP_Vector[(int)FRAME.ELB_R], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.ELB_R], BP_Vector[(int)FRAME.RST_R], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.RST_R], BP_Vector[(int)FRAME.HND_R], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.SHO_L], BP_Vector[(int)FRAME.LEG_L], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.SHO_R], BP_Vector[(int)FRAME.LEG_R], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.LEG_L], BP_Vector[(int)FRAME.LEG_R], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.LEG_L], BP_Vector[(int)FRAME.KNE_L], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.KNE_L], BP_Vector[(int)FRAME.ANC_L], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.ANC_L], BP_Vector[(int)FRAME.FOT_L], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.LEG_R], BP_Vector[(int)FRAME.KNE_R], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.KNE_R], BP_Vector[(int)FRAME.ANC_R], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawCapsule3D(BP_Vector[(int)FRAME.ANC_R], BP_Vector[(int)FRAME.FOT_R], sizeD, 10, DX.GetColor(0, 255, 0), DX.GetColor(0, 255, 0), 1);
			DX.DrawTriangle3D(BP_Vector[(int)FRAME.TRS], BP_Vector[(int)FRAME.SHO_L], BP_Vector[(int)FRAME.SHO_R], DX.GetColor(0, 255, 0), 1);
			DX.DrawTriangle3D(BP_Vector[(int)FRAME.TRS], BP_Vector[(int)FRAME.SHO_R], BP_Vector[(int)FRAME.LEG_R], DX.GetColor(0, 255, 0), 1);
			DX.DrawTriangle3D(BP_Vector[(int)FRAME.TRS], BP_Vector[(int)FRAME.LEG_R], BP_Vector[(int)FRAME.LEG_L], DX.GetColor(0, 255, 0), 1);
			DX.DrawTriangle3D(BP_Vector[(int)FRAME.TRS], BP_Vector[(int)FRAME.LEG_L], BP_Vector[(int)FRAME.SHO_L], DX.GetColor(0, 255, 0), 1);
			
			return 0;
		}
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
        static void Main()
        {
            //Application.EnableVisualStyles();     //visualスタイルの有効化    
												  //フォーム生成
			//Form表示
			Form1 form = new Form1();
			form.Show();
			//Kinect
			KinectControl kinectControl = new KinectControl(0);
			
			int c = 0;
			Player player = new PlayerMiku(DX.MGetTranslate(DX.VGet(0, 0, 0)));
			int[] key = new int[256];
			Random rnd = new Random();
			while (form.Created)   //フォームがある間ループ
            {
				//キー
				GetHitKeyStateAll_2(ref key);
				//行動
				player.next();

				//描画
				DX.ClsDrawScreen();
				//カメラ
				DX.SetCameraPositionAndTarget_UpVecY(player.camera(),DX.VGet(0,10,0));
				//プレイヤー
				DX.VECTOR[] BP_Vector = skeletonKinect2DXLib(kinectControl.getSkeletonData());
				player.CopyBonePos(BP_Vector);
				player.draw();
				player.setFace(
					kinectControl.getAnimationUnit(AnimationUnit.JawLower),
					0,
					kinectControl.getAnimationUnit(AnimationUnit.LipCornerDepressor)
					);
				//ステージ
				{
					DX.DrawTriangle3D(DX.VGet(-100, 0, -100), DX.VGet(100, 0, -100), DX.VGet(-100, 0, 100), DX.GetColor(15, 55, 150), 1);
					DX.DrawTriangle3D(DX.VGet(100, 0, 100), DX.VGet(100, 0, -100), DX.VGet(-100, 0, 100), DX.GetColor(15, 55, 150), 1);
				}
				//デバグ
				DrawBone(player.framePoint);
				//DrawBone(BP_Vector);
				//DrawBone(player.defaultFramePoint);
				DX.DrawString(0, 0, kinectControl.getAnimationUnit(AnimationUnit.JawLower).ToString(), 0xFFFFFF);
				DX.DrawString(0, 32, kinectControl.getAnimationUnit(AnimationUnit.LipCornerDepressor).ToString(), 0xFFFFFF);
				//描画終了
				DX.ScreenCopy();
                //自作関数。
                Application.DoEvents();
            }
        }
    }
}
