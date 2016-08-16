using System;
using Microsoft.Kinect;
using Microsoft.Kinect.Toolkit.FaceTracking;

namespace csFormDX
{
	class KinectControl
	{
		//Kinectクラス
		KinectSensor kinect = null;
		FaceTracker faceTracker;
		//データ格納用配列
		Skeleton skeletonData;
		byte[] colorData;
		short[] depthData;
		EnumIndexableCollection<AnimationUnit, float> animationUnits;
		public KinectControl(int num)
		{
			kinect = KinectSensor.KinectSensors[num];
			if (kinect != null)
			{
				kinect.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(kinectEvent);
				kinect.ColorStream.Enable();
				kinect.DepthStream.Enable();
				kinect.SkeletonStream.Enable();
				// Kinectの開始
				kinect.Start();

			}
		}
		~KinectControl()
		{
			if (kinect != null)
			{
				// Kinectの停止
				kinect.Dispose();
				kinect = null;
			}
		}
		//データ取得イベント
		private void kinectEvent(object sender, AllFramesReadyEventArgs e)
		{
			//skeleton
			var skeletonFrame = e.OpenSkeletonFrame();
			if (skeletonFrame != null)
			{
				Skeleton[] skeletonDatas = new Skeleton[skeletonFrame.SkeletonArrayLength];
				skeletonFrame.CopySkeletonDataTo(skeletonDatas);
				foreach (Skeleton skeletonData in skeletonDatas)
				{
					if (skeletonData.TrackingState != SkeletonTrackingState.NotTracked)
					{
						this.skeletonData = skeletonData;
					}
				}
			}
			//color
			var colorImageFrame = e.OpenColorImageFrame();
			if (colorImageFrame != null)
			{
				if (colorData == null) colorData = new byte[colorImageFrame.PixelDataLength];
				colorImageFrame.CopyPixelDataTo(colorData);
			}
			//depth
			var depthImageFrame = e.OpenDepthImageFrame();
			if (depthImageFrame != null)
			{
				if(depthData == null) depthData = new short[depthImageFrame.PixelDataLength];
				depthImageFrame.CopyPixelDataTo(depthData);
			}
			//face
			if (faceTracker == null)
			{
				try
				{
					faceTracker = new FaceTracker(kinect);
				}
				catch (InvalidOperationException)
				{
					faceTracker = null;
				}
			}
			if (faceTracker != null)
			{
				var faceTrackFrame = faceTracker.Track(kinect.ColorStream.Format, colorData, kinect.DepthStream.Format, depthData, skeletonData);
				if (faceTrackFrame.TrackSuccessful)
				{
					animationUnits = faceTrackFrame.GetAnimationUnitCoefficients();
				}
			}
		}
		public Skeleton getSkeletonData()
		{
			return skeletonData;
		}
		//表情
		public float getAnimationUnit(AnimationUnit animationUnit)
		{
			if (animationUnits == null) return 0;
			return animationUnits[animationUnit];
		}
	}
}
