using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
public class JumpingJackColliderContainer : MonoBehaviour
{
	public delegate void JumpingJackEvents (ColliderType _type);
	public static JumpingJackEvents OnJumpingJackColliderHit;
	public delegate void JumpingJackStatusEvents ();
	public static JumpingJackStatusEvents OnJumpingJackStart;
	public static JumpingJackStatusEvents OnJumpingJackFinish;

	public Animator modelAnimator;
	public Transform target;
	public TextMeshProUGUI header;
	public Transform topColliders, bottomColliders;

	private PlayerPackage _playerPackage;
	private DataRecorder _dataRecorder;
	private float _timer, _goodTimer;
	private bool _isDoneProperJacks, _startCountJacks, _startJumpingJacks;
	private int _jumpingJackCount; //how many they have done
	private int _remaining; //how many they have left
	private int _targetJumpingJacks = 10;

	void OnEnable ()
	{
		JumpingJackColliderContainer.OnJumpingJackColliderHit += _OnJumpingJackColliderHit;

	}
	void OnDisable ()
	{
		JumpingJackColliderContainer.OnJumpingJackColliderHit += _OnJumpingJackColliderHit;
	}

	public void StartJumpingjacks (float delay)
	{
     
		_playerPackage = FindObjectOfType<PlayerPackage> ();
		_dataRecorder = FindObjectOfType<DataRecorder> ();
		_remaining = _targetJumpingJacks;
        if (gameObject.activeSelf == false) return;
        StopCoroutine ("IStartJumpingjacks");
		StartCoroutine ("IStartJumpingjacks", delay);
	}

	IEnumerator IStartJumpingjacks (float delay)
	{
		float timer = delay;
		header.text = "Start Jumping Jacks in ... " + timer.ToString ("F0");
		while (timer > 0f)
		{
			header.text = "Start Jumping Jacks in ... " + timer.ToString ("F0");
			timer -= Time.unscaledDeltaTime;
			yield return null;
		}
		_dataRecorder.StartRecording ();
		_startJumpingJacks = true;
		modelAnimator.SetBool ("doJumpingJacks", true);
	}

	void Update ()
	{
		transform.position = target.position;
		if (_playerPackage.activeSceneType != SceneType.Excercise) return;
		if (_startJumpingJacks)
		{
			_timer += Time.unscaledDeltaTime;
			if (_timer > 1f)
			{
				header.text = "Do jumping jacks";
				_goodTimer = 0;
				_startCountJacks = false;
			}
			else if (!_isDoneProperJacks)
			{
				header.text = "Do proper jumping jacks";
				_goodTimer = 0;
				_startCountJacks = false;
			}
			else
			{
				header.text = "Remaining: " + _remaining;
				_goodTimer += Time.unscaledDeltaTime;
				if (_goodTimer > 1f)
				{
					_startCountJacks = true;
				}
			}

			if (_jumpingJackCount >= _targetJumpingJacks && _startJumpingJacks)
			{
				_playerPackage.LoadNextScene ();
				_startJumpingJacks = false;
				_dataRecorder.StopRecording ("Excercise");
			}

			//Count Down for jumping jacks
			// if (_remaining <= 0 && _startJumpingJacks)
			// {
			// 	_startJumpingJacks = false;
			// }

		}
		else
		{
			modelAnimator.SetBool ("doJumpingJacks", false);
			header.text = "";
		}
	}

	public void _OnJumpingJackColliderHit (ColliderType type)
	{
		if (!_startJumpingJacks) { return; }
		if (_playerPackage.activeSceneType != SceneType.Excercise) return;
		bottomColliders.gameObject.SetActive (false);
		topColliders.gameObject.SetActive (false);

		if (_timer < .1f)
		{
			_isDoneProperJacks = false;
		}
		else
		{
			_isDoneProperJacks = true;
			if (type == ColliderType.TOP && _startCountJacks)
			{
				_jumpingJackCount++;
				_remaining--;
			}
		}

		_timer = 0f;

		switch (type)
		{
			case ColliderType.BOTTOM:
				{
					topColliders.gameObject.SetActive (true);
					break;
				}
			case ColliderType.TOP:
				{
					bottomColliders.gameObject.SetActive (true);

					break;
				}
		}
	}
}