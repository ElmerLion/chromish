using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;

    private Transform barTransform;
    private Transform separatorContainer;
    private TextMeshPro healthAmount;

    private void Awake() {
        barTransform = transform.GetChild(2);
        healthAmount = transform.Find("HealthAmount").GetComponent<TextMeshPro>();
    }

    private void Start() {
        UpdateBar();
        UpdateHealthBarVisibility();
        //ConstructHealthbarSeparators();

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealthAmountMaxChanged += HealthSystem_OnHealthAmountMaxChanged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
        healthSystem.OnDied += HealthSystem_OnDied;

    }

    private void HealthSystem_OnDied(object sender, System.EventArgs e) {
        transform.gameObject.SetActive(false);
    }

    private void Update() {
        Vector3 lookAtPosition = new Vector3(Player.Instance.transform.position.x, transform.position.y, Player.Instance.transform.position.z);
        transform.LookAt(lookAtPosition);
    }

    private void HealthSystem_OnHealthAmountMaxChanged(object sender, System.EventArgs e) {
        //ConstructHealthbarSeparators();
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e) {
        UpdateHealthAmount();
        UpdateBar();
        UpdateHealthBarVisibility();
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e) {
        UpdateHealthAmount();
        UpdateBar();
        UpdateHealthBarVisibility();
    }

    private void UpdateHealthAmount() {
        healthAmount.text = healthSystem.GetCurrentHealthAmount().ToString() + "/" + healthSystem.GetMaxHealthAmount().ToString();
    }

    private void ConstructHealthbarSeparators() {
        separatorContainer = transform.Find("SeparatorContainer");
        Transform separatorTemplate = separatorContainer.Find("SeparatorTemplate");
        separatorTemplate.gameObject.SetActive(false);

        foreach (Transform separatorTransform in separatorContainer) {
            if (separatorTransform == separatorTemplate) continue;
            Destroy(separatorTransform.gameObject);
        }

        int healthAmountPerSeparator = 10;
        float barSize = 3f;
        float barOneHealthAmountSize = barSize / healthSystem.GetMaxHealthAmount();
        int healthSeparatorCount = Mathf.FloorToInt(healthSystem.GetMaxHealthAmount() / healthAmountPerSeparator);

        for (int i = 1; i < healthSeparatorCount; i++) {

            Transform separatorTransform = Instantiate(separatorTemplate, separatorContainer);
            separatorTransform.gameObject.SetActive(true);
            separatorTransform.localPosition = new Vector3(barOneHealthAmountSize * i * healthAmountPerSeparator, 0, 0);

        }
    }

    private void UpdateBar() {
        barTransform.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
    }

    private void UpdateHealthBarVisibility() {
        if (healthSystem.IsFullHealth()) {
            gameObject.SetActive(false);
        } else {
            gameObject.SetActive(true);
        }
        //gameObject.SetActive(true);
    }
}
