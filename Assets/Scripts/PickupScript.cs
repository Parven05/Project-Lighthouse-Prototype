using UnityEngine;

public class PickupScript : MonoBehaviour
{
    [SerializeField] private LayerMask pickupMask;
    [SerializeField] private Camera playerCam;
    [SerializeField] private Transform pickupTarget;
    [SerializeField] private float pickupRange;
    [SerializeField] private float pickupSpeed = 5f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float pickupDistance = 0.2f;
    [SerializeField] private float returnSpeed = 3f;
    [SerializeField] private GameObject dof;
    [SerializeField] private FirstPersonController player;

    private Rigidbody currentObject;
    private Vector3 initialObjectPosition;
    private Quaternion initialObjectRotation;
    private Vector3 initialObjectScale;
    private Vector3 initialCameraForward;
    private bool canMoveObject = true;

    private void Start()
    {
        initialObjectScale = Vector3.one;
        dof.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (currentObject)
            {
                ReturnObjectToInitialPosition();

                player.enabled = true;
               // player.cameraCanMove = true;
                dof.SetActive(false);
               // player.mouseSensitivity = 2f;
                canMoveObject = true;

                currentObject.useGravity = true;
                currentObject = null;
            }
            else
            {
                Ray cameraRay = playerCam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
                if (Physics.Raycast(cameraRay, out RaycastHit hitInfo, pickupRange, pickupMask))
                {
                    player.enabled = false;
                   // player.cameraCanMove = false;
                    dof.SetActive(true);
                  //  player.mouseSensitivity = 0.3f;
                    currentObject = hitInfo.rigidbody;
                    currentObject.useGravity = false;
                    initialObjectPosition = currentObject.transform.position;
                    initialObjectRotation = currentObject.transform.rotation;
                    initialCameraForward = playerCam.transform.forward;
                    canMoveObject = true;
                }
            }
        }

        // Rotate the picked up object using Q and E keys
        if (currentObject && canMoveObject)
        {
            float rotateAmount = Input.GetKey(KeyCode.Q) ? -1f : Input.GetKey(KeyCode.E) ? 1f : 0f;
            float rotateX = rotateAmount * rotationSpeed;

            // Modify this line to rotate upside down using the E key
            float rotateY = Input.GetKey(KeyCode.E) ? 1f : 0f;

            Vector3 currentCameraForward = playerCam.transform.forward;

            // Calculate the rotation between initial and current camera forward
            Quaternion cameraRotation = Quaternion.FromToRotation(initialCameraForward, currentCameraForward);

            currentObject.transform.Rotate(Vector3.up, rotateX);
            currentObject.transform.rotation *= cameraRotation; // Apply camera rotation
            currentObject.transform.Rotate(Vector3.forward, rotateY); // Rotate upside down

            Vector3 targetPosition = pickupTarget.position + currentCameraForward * pickupDistance;
            Vector3 newPosition = Vector3.Lerp(currentObject.position, targetPosition, Time.deltaTime * pickupSpeed);

            Vector3 centerScreenPoint = playerCam.WorldToViewportPoint(pickupTarget.position);
            Vector3 newScreenPoint = playerCam.WorldToViewportPoint(newPosition);
            float distanceToCenter = Vector3.Distance(centerScreenPoint, newScreenPoint);

            if (distanceToCenter < 0.05f)
            {
                newPosition = targetPosition;
                canMoveObject = false;
            }

            currentObject.position = newPosition;
        }
        else if (currentObject && !canMoveObject)
        {
            Vector3 newPosition = Vector3.Lerp(currentObject.position, initialObjectPosition, Time.deltaTime * returnSpeed);
            currentObject.position = newPosition;

            Quaternion newRotation = Quaternion.Lerp(currentObject.rotation, initialObjectRotation, Time.deltaTime * returnSpeed);
            currentObject.rotation = newRotation;
        }
    }

    private void ReturnObjectToInitialPosition()
    {
        canMoveObject = false;
        initialObjectRotation = Quaternion.identity;
    }
}
