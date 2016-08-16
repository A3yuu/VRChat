using System;
using DxLibDLL;
using System.Collections.Generic;

namespace VRChat
{
	//クラス
	abstract class Object
	{
		public DX.VECTOR pos;
		//readonly DX.VECTOR rot;
		public DX.MATRIX mat;
		public int t = 0;
		public virtual bool next()
		{
			t++;
			pos = DX.VGet(0, 0, 0) * mat;
			return false;
		}
		public virtual void draw()
		{
		}
	};
	abstract class Player : Object
	{
		//番号から親番号
		public static int[] oyaJointNum = new int[]{
			0,
			0,
			1,
			2,
			2,
			4,
			5,
			6,
			2,
			8,
			9,
			10,
			0,
			12,
			13,
			14,
			15,
			12,
			17,
			18,
			19,
		};
		public static int[] childJointNum = new int[]{
			1,
			2,
			3,
			-1,
			5,
			6,
			7,
			-1,
			9,
			10,
			11,
			-1,
			-1,
			14,
			15,
			16,
			-1,
			18,
			19,
			20,
			-1,
		};
		//フレームの識別
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
		//定数
		//変数
		//モデル
		public int model;
		//フレーム
		public int[] frame;
		public DX.VECTOR skeletonCenter;
		public DX.VECTOR[] defaultFramePoint;
		public DX.MATRIX[] defaultFrameMatrix;
		public float[] frameSize;
		public DX.VECTOR[] framePoint;
		//シェイプ
		public int[] shape;
		//アニメーション
		public int animationNum;
		public int[] animation;
		public float animationTime;
		public float animationTimeMax;
		//移行時間
		public int timeTransition = 0;
		public int nowMotion;
		//Motionフラグ
		bool motionFlag = false;
		public Player(DX.MATRIX mat)
		{
			this.mat = mat;
		}
		public DX.VECTOR camera()
		{
			return DX.VGet(0, 10, -5) * mat;
		}
		public override bool next()
		{
			base.next();
			//animation
			//if (!motionFlag)
			//{
			//	if (nowMotion != animationNomal)
			//	{
			//		nowMotion = animationNomal;
			//		timeTransition = 0;
			//	}
			//}
			//motionFlag = false;
			//if (timeTransition < 10) timeTransition++;
			//animationTime += 0.033333f;
			//for (int i = 0; i < animationNum; i++)
			//{
			//	float a = DX.MV1GetAttachAnimBlendRate(model, animation[i]);
			//	a *= (10f - timeTransition) / (11f - timeTransition);
			//	DX.MV1SetAttachAnimBlendRate(model, animation[i], a);
			//}
			//DX.MV1SetAttachAnimBlendRate(model, animation[nowMotion], timeTransition * 0.1f);
			return false;
		}
		//ジョイントのワールド座標からフレーム変換行列を知りたい
		public static DX.MATRIX CopyMatrixNI2DX(DX.VECTOR ni, DX.VECTOR dx, DX.MATRIX FrameLocalWorldMatrix)
		{
			DX.VECTOR hoge = DX.VTransform(ni, DX.MInverse(FrameLocalWorldMatrix));
			DX.VECTOR hoge2 = DX.VTransform(dx, DX.MInverse(FrameLocalWorldMatrix));

			DX.MATRIX h = DX.MGetRotVec2(hoge2, hoge);
			if (float.IsNaN(h.m00)) h = DX.MGetIdent();
			return h;
		}
		//座標の適用
		public void CopyBonePos(DX.VECTOR[] skeletonDatas)
		{
			//位置合わせ
			//framePoint[(int)FRAME.CENTER] = (skeletonDatas[(int)FRAME.TRS_UND] * 10) + skeletonCenter;
			framePoint[(int)FRAME.CENTER] = DX.VGet(skeletonDatas[(int)FRAME.CENTER].x * 10, defaultFramePoint[(int)FRAME.CENTER].y, skeletonDatas[(int)FRAME.CENTER].z * 10);
			framePoint[(int)FRAME.TRS] = framePoint[(int)FRAME.CENTER] + defaultFramePoint[(int)FRAME.TRS] - defaultFramePoint[(int)FRAME.CENTER];
			framePoint[(int)FRAME.TRS_UND] = framePoint[(int)FRAME.CENTER] + defaultFramePoint[(int)FRAME.TRS_UND] - defaultFramePoint[(int)FRAME.CENTER];
			for (int i = 0; i < (int)FRAME.MAX; i++)
			{
				if (i != (int)FRAME.CENTER && i != (int)FRAME.TRS && i != (int)FRAME.TRS_UND)
				{
					framePoint[i] = DX.VNorm(skeletonDatas[i] - skeletonDatas[oyaJointNum[i]]);
					framePoint[i] *= frameSize[i];
					framePoint[i] += framePoint[oyaJointNum[i]];
				}
			}
			//初期化
			for (int i = 0; i < (int)FRAME.MAX; i++)
			{
				DX.MV1ResetFrameUserLocalMatrix(model, frame[i]);
			}
			//座標変換
			DX.MV1SetFrameUserLocalMatrix(model, frame[(int)FRAME.CENTER], DX.MGetTranslate(framePoint[(int)FRAME.CENTER]));
			DX.MATRIX[] frameMatrix = new DX.MATRIX[(int)FRAME.MAX];
			for (int i = 0; i < (int)FRAME.MAX; i++)
			{
				if (childJointNum[i] == -1)
				{
					frameMatrix[i] = DX.MGetIdent();
				}
				else
				{
					frameMatrix[i] = CopyMatrixNI2DX(framePoint[childJointNum[i]], DX.MV1GetFramePosition(model, frame[childJointNum[i]]), DX.MV1GetFrameLocalWorldMatrix(model, frame[i]));
				}
				//体の傾き
				if (i == (int)FRAME.TRS)
				{
					//float uptrs = (float)Math.PI * 0.5f - (float)Math.Acos(DX.VDot(DX.VGet(0, 0, -1), DX.VNorm(framePoint[(int)FRAME.SHO_L] - framePoint[(int)FRAME.SHO_R])));
					float uptrs = (float)Math.PI * 0.5f - (float)Math.Acos(-DX.VNorm(framePoint[(int)FRAME.SHO_L] - framePoint[(int)FRAME.SHO_R]).z);
					if (!float.IsNaN(uptrs)) frameMatrix[i] = DX.MMult(DX.MGetRotY(uptrs), frameMatrix[i]);
				}
				if (i == (int)FRAME.TRS_UND)
				{
					//float undtrs = (float)Math.PI * 0.5f - (float)Math.Acos(DX.VDot(DX.VGet(0, 0, -1), DX.VNorm(framePoint[(int)FRAME.LEG_L] - framePoint[(int)FRAME.LEG_R])));
					float undtrs = (float)Math.PI * 0.5f - (float)Math.Acos(-DX.VNorm(framePoint[(int)FRAME.LEG_L] - framePoint[(int)FRAME.LEG_R]).z);
					if (!float.IsNaN(undtrs)) frameMatrix[i] = DX.MMult(DX.MGetRotY(undtrs), frameMatrix[i]);
				}
				//適用
				frameMatrix[i] = DX.MMult(frameMatrix[i], DX.MV1GetFrameLocalMatrix(model, frame[i]));
				DX.MV1SetFrameUserLocalMatrix(model, frame[i], frameMatrix[i]);
			}
		}
		//表情の適用
		public void setFace(float mouth, float eye, float eyebrow)
		{
			DX.MV1SetShapeRate(model, shape[0], mouth);
			DX.MV1SetShapeRate(model, shape[1], eye);
			if (eyebrow < 0) DX.MV1SetShapeRate(model, shape[2], eyebrow);
			else DX.MV1SetShapeRate(model, shape[2], 0);
			if (eyebrow > 0) DX.MV1SetShapeRate(model, shape[3], eyebrow);
			else DX.MV1SetShapeRate(model, shape[3], 0);
		}
		public override void draw()
		{
			DX.MV1SetMatrix(model, mat);
			//物理演算
			DX.MV1PhysicsCalculation(model, 33.333f);
			DX.MV1DrawModel(model);
		}
	};
	class PlayerMiku : Player
	{
		//MMDのボーン名
		static readonly string[] frameName = {
			"センター",
			"上半身",
			"首",
			"頭",
			"左腕",
			"左ひじ",
			"左手首",
            "左手先",
            "右腕",
			"右ひじ",
			"右手首",
            "右手先",
            "下半身",
			"左足",
			"左ひざ",
			"左足首",
			"左つま先",
			"右足",
			"右ひざ",
			"右足首",
			"右つま先",
		};
		//シェイプ名
		static readonly string[] shapeName = {
			"あ",
			"まばたき",
			"上",
			"下",
		};
		public PlayerMiku(DX.MATRIX mat) : base(mat)
		{
			model = DX.MV1LoadModel("data/player/miku/model.pmd");
			//物理演算
			DX.MV1PhysicsResetState(model);
			//アニメーション
			//animationNum = DX.MV1GetAnimNum(model);
			//for (int i = 0; i < animationNum; i++) animation[i] = DX.MV1AttachAnim(model, i, -1, DX.FALSE);
			//animationTime = 0;
			//animationTimeMax = DX.MV1GetAttachAnimTotalTime(model, animation[0]);
			//フレームナンバー登録
			frame = new int[frameName.Length];
			for (int i = 0; i < frame.Length; i++)
			{
				frame[i] = DX.MV1SearchFrame(model, frameName[i]);
			}
			//shapeナンバー登録
			shape = new int[shapeName.Length];
			for (int i = 0; i < shape.Length; i++)
			{
				shape[i] = DX.MV1SearchShape(model, shapeName[i]);
			}
			//フレーム座標
			defaultFramePoint = new DX.VECTOR[frameName.Length];
			framePoint = new DX.VECTOR[frameName.Length];
			for (int i = 0; i < defaultFramePoint.Length; i++)
			{
				defaultFramePoint[i] = DX.MV1GetFramePosition(model, frame[i]);
			}
			defaultFrameMatrix = new DX.MATRIX[frameName.Length];
			for (int i = 0; i < defaultFrameMatrix.Length; i++)
			{
				defaultFrameMatrix[i] = DX.MV1GetFrameLocalMatrix(model, frame[i]);
			}
			//中心位置設定
			skeletonCenter = defaultFramePoint[(int)FRAME.CENTER] - defaultFramePoint[(int)FRAME.TRS_UND];
			if (float.IsNaN(skeletonCenter.y)) skeletonCenter = DX.VGet(0, -4.0f, 0);
			//フレームサイズ
			frameSize = new float[frameName.Length];
			for (int i = 0; i < (int)FRAME.MAX; i++)
			{
				frameSize[i] = DX.VSize(defaultFramePoint[i] - defaultFramePoint[oyaJointNum[i]]);
			}
		}
	}
	class PlayerHubuki : Player
	{
		//MMDのボーン名
		static readonly string[] frameName = {
			"全ての親",
			//"センター",
			"上半身",
			"首",
			"ヘッドセット０",
			"左腕",
			"左ひじ",
			"左手首",
			"左人指３",
            //"左手先",
            "右腕",
			"右ひじ",
			"右手首",
			"右人指３",
            //"右手先",
            "下半身",
			"左足",
			"左ひざ",
			"左足首",
			"左つま先",
			"右足",
			"右ひざ",
			"右足首",
			"右つま先",
		};
		public PlayerHubuki(DX.MATRIX mat) : base(mat)
		{
			model = DX.MV1LoadModel("data/player/hubuki/model.pmx");
			//物理演算
			DX.MV1PhysicsResetState(model);
			//アニメーション
			//animationNum = DX.MV1GetAnimNum(model);
			//for (int i = 0; i < animationNum; i++) animation[i] = DX.MV1AttachAnim(model, i, -1, DX.FALSE);
			//animationTime = 0;
			//animationTimeMax = DX.MV1GetAttachAnimTotalTime(model, animation[0]);
			//フレームナンバー登録
			frame = new int[frameName.Length];
			for (int i = 0; i < frame.Length; i++)
			{
				frame[i] = DX.MV1SearchFrame(model, frameName[i]);
			}
			//フレーム座標
			defaultFramePoint = new DX.VECTOR[frameName.Length];
			framePoint = new DX.VECTOR[frameName.Length];
			for (int i = 0; i < defaultFramePoint.Length; i++)
			{
				defaultFramePoint[i] = DX.MV1GetFramePosition(model, frame[i]);
			}
			//中心位置設定
			skeletonCenter = defaultFramePoint[(int)FRAME.CENTER] - defaultFramePoint[(int)FRAME.TRS_UND];
			if (float.IsNaN(skeletonCenter.y)) skeletonCenter = DX.VGet(0, -4.0f, 0);
			//フレームサイズ
			frameSize = new float[frameName.Length];
			for (int i = 0; i < (int)FRAME.MAX; i++)
			{
				frameSize[i] = DX.VSize(defaultFramePoint[i] - defaultFramePoint[oyaJointNum[i]]);
			}
		}
	}
}