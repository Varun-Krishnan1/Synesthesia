using System.Collections;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class particleAttractorLinear : MonoBehaviour {
	ParticleSystem ps;
	ParticleSystem.Particle[] m_Particles;
	public Transform target;
	public float speed = 5f;
	int numParticlesAlive;
	public float duration = 2f; 

	private float startTime; 
	void Start () {
		ps = GetComponent<ParticleSystem>();
		if (!GetComponent<Transform>()){
			GetComponent<Transform>();
		}
		startTime = Time.time; 
	}
	void Update () {
		m_Particles = new ParticleSystem.Particle[ps.main.maxParticles];
		numParticlesAlive = ps.GetParticles(m_Particles);
		float step = speed * Time.deltaTime;
		for (int i = 0; i < numParticlesAlive; i++) {
			m_Particles[i].position = Vector3.LerpUnclamped(m_Particles[i].position, target.position, step);
		}
		ps.SetParticles(m_Particles, numParticlesAlive);

		if(Time.time - startTime > duration)
        {
			target.gameObject.SetActive(true);
			Debug.Log(target.GetComponentsInChildren<Transform>());
			foreach (Transform child in target.GetComponentsInChildren<Transform>(true)) // use true parameter to get inactive
			{
				Debug.Log(child);
				child.gameObject.SetActive(true);
			}

			Destroy(this.gameObject);
		}
	}
}
