var target = Vector3.zero;
var damping = 6.0;
var smooth = true;

@script AddComponentMenu("Camera-Control/Smooth Look At")

function LateUpdate () {
	if (target != Vector2.zero) {
		if (smooth)
		{
			// Look at and dampen the rotation
			var rotation = Quaternion.LookRotation(target - transform.position);
			transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * damping);
		}
		else
		{
			// Just lookat
		    transform.LookAt(target);
		}
	}
}

function Start () {
	// Make the rigid body not change rotation
   	if (rigidbody)
		rigidbody.freezeRotation = true;
}