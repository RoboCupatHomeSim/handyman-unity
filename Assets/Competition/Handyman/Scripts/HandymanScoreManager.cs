using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using SIGVerse.Common;
using SIGVerse.ToyotaHSR;
using UnityEngine.EventSystems;

namespace SIGVerse.Competition.Handyman
{
	public static class Score
	{
		public const int MaxScore = +999;
		public const int MinScore = -999;

		public enum Type
		{
			RoomReachedSuccess,
			ObjectGraspedSuccess,
			ComeBackSuccess,
			HsrCollisionEnter,
			ObjectCollisionEnter,
		}

		public static int GetScore(Type scoreType, params object[] args)
		{
			switch(scoreType)
			{
				case Score.Type.RoomReachedSuccess  : { return +20; }
				case Score.Type.ObjectGraspedSuccess: { return +50; }
				case Score.Type.ComeBackSuccess     : { return +30; }
				case Score.Type.HsrCollisionEnter   : { return -10; }
				case Score.Type.ObjectCollisionEnter: { return GetObjectCollisionScore((Collision)args[0]); }
			}

			throw new Exception("Illegal score type. Type = " + (int)scoreType + ", method name=(" + System.Reflection.MethodBase.GetCurrentMethod().Name + ")");
		}

		public static float GetObjectCollisionVeloticyThreshold()
		{
			return 1.0f;
		}

		private static int GetObjectCollisionScore(Collision collision)
		{
			return Mathf.FloorToInt((collision.relativeVelocity.magnitude - 1.0f) * -10);
		}
	}


	public class HandymanScoreManager : MonoBehaviour, IHSRCollisionHandler, ITransferredCollisionHandler
	{
		private const float DefaultTimeScale = 1.0f;

		public List<GameObject> scoreNotificationDestinations;

		//---------------------------------------------------
		private GameObject mainMenu;

		private int score;


		void Awake()
		{
			this.mainMenu = GameObject.FindGameObjectWithTag("MainMenu");
		}

		// Use this for initialization
		void Start()
		{
			this.UpdateScoreText(0, HandymanConfig.Instance.GetTotalScore());
			
			this.score = 0;

			Time.timeScale = 0.0f;
		}


		public void AddScore(Score.Type scoreType, params object[] args)
		{
			int additionalScore = Score.GetScore(scoreType, args);

			this.score = Mathf.Clamp(this.score + additionalScore, Score.MinScore, Score.MaxScore);

			this.UpdateScoreText(this.score);

			SIGVerseLogger.Info("Score add [" + additionalScore + "], Challenge " + HandymanConfig.Instance.numberOfTrials + " Score=" + this.score);

			// Send the Score Notification
			ScoreStatus scoreStatus = new ScoreStatus(additionalScore, this.score, HandymanConfig.Instance.GetTotalScore());

			foreach(GameObject scoreNotificationDestination in this.scoreNotificationDestinations)
			{
				ExecuteEvents.Execute<IScoreHandler>
				(
					target: scoreNotificationDestination,
					eventData: null,
					functor: (reciever, eventData) => reciever.OnScoreChange(scoreStatus)
				);
			}
		}

		public void TaskStart()
		{
			this.UpdateScoreText(this.score);

			Time.timeScale = HandymanScoreManager.DefaultTimeScale;
		}

		public void TaskEnd()
		{
			Time.timeScale = 0.0f;

			HandymanConfig.Instance.AddScore(this.score);

			this.UpdateScoreText(this.score, HandymanConfig.Instance.GetTotalScore());

			SIGVerseLogger.Info("Total Score=" + HandymanConfig.Instance.GetTotalScore().ToString());

			HandymanConfig.Instance.RecordScoreInFile();
		}


		private void UpdateScoreText(float score)
		{
			ExecuteEvents.Execute<IPanelScoreHandler>
			(
				target: this.mainMenu,
				eventData: null,
				functor: (reciever, eventData) => reciever.OnScoreChange(score)
			);
		}

		private void UpdateScoreText(float score, float total)
		{
			ExecuteEvents.Execute<IPanelScoreHandler>
			(
				target: this.mainMenu,
				eventData: null,
				functor: (reciever, eventData) => reciever.OnScoreChange(score, total)
			);
		}


		public void OnHsrCollisionEnter(Vector3 contactPoint)
		{
			this.AddScore(Score.Type.HsrCollisionEnter);
		}


		public void OnTransferredCollisionEnter(Collision collision, GameObject collidingObject)
		{
			SIGVerseLogger.Info("Object collision occurred. name=" + collidingObject.name);

			this.AddScore(Score.Type.ObjectCollisionEnter, collision);
		}
	}
}

