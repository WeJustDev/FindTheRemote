using UnityEngine;

public class RamasserRubis : MonoBehaviour
{
    public Transform mainDuPersonnage;
    public float forceLancer = 10f;
    public float distanceMaxRamassage = 3f; // Distance maximale de ramassage

    private GameObject objetEnMain;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (objetEnMain != null)
            {
                LancerObjet();
            }
            else
            {
                RamasserObjet();
            }
        }
    }

    void RamasserObjet()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, distanceMaxRamassage)) // Ajouter la distance maximale
        {
            if (hit.collider.CompareTag("Rubis"))
            {
                objetEnMain = hit.collider.gameObject;
                objetEnMain.transform.SetParent(mainDuPersonnage);
                objetEnMain.transform.localPosition = Vector3.zero;
                objetEnMain.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    void LancerObjet()
    {
        objetEnMain.transform.SetParent(null);
        Rigidbody rb = objetEnMain.GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(Camera.main.transform.forward * forceLancer, ForceMode.Impulse);
        objetEnMain = null;
    }
}
