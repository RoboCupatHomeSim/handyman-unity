using SIGVerse.ToyotaHSR;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SIGVerse.Competition.Handyman
{
	public class HandymanHsrInit : MonoBehaviour
	{
		public GameObject rosBridgeScripts;

		private void Awake()
		{
			if (HandymanConfig.Instance.configFileInfo.isGraspableObjectsPositionRandom && HandymanConfig.Instance.configFileInfo.reduceLoadInDataGen)
			{
				this.ReduceLoad();
			}
		}

		private void ReduceLoad()
		{
			this.rosBridgeScripts.GetComponentInChildren<HSRPubXtionDepthController>().sendingInterval *= 1000;
			this.rosBridgeScripts.GetComponentInChildren<HSRPubXtionRGBController>()  .sendingInterval *= 1000;
			this.rosBridgeScripts.GetComponentInChildren<HSRPubStereoRGBController>() .sendingInterval *= 1000;
			this.rosBridgeScripts.GetComponentsInChildren<HSRPubWideRGBController>().ToList().ForEach(x => x.sendingInterval *= 1000);
		}
	}
}

