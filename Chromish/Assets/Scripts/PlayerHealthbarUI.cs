using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthbarUI : MonoBehaviour {

    [SerializeField] private Image Healthbarhealth;
    [SerializeField] private TextMeshProUGUI healthAmount;
    private HealthSystem playerHealthSystem;


    private void Start() {
        playerHealthSystem = Player.Instance.transform.GetComponent<HealthSystem>();
        UpdateHealthUI();

        playerHealthSystem.OnDamaged += PlayerHealthSystem_OnDamaged;
        playerHealthSystem.OnHealed += PlayerHealthSystem_OnHealed;
    }

    private void PlayerHealthSystem_OnHealed(object sender, System.EventArgs e) {
        UpdateHealthUI();
    }

    private void PlayerHealthSystem_OnDamaged(object sender, System.EventArgs e) {
        UpdateHealthUI();
    }

    private void UpdateHealthUI() {
        Healthbarhealth.fillAmount = playerHealthSystem.GetHealthAmountNormalized();
        UpdateHealthAmount();

        if (Healthbarhealth.fillAmount > 0.4f) {
            Healthbarhealth.color = Color.green;
        }
        if (Healthbarhealth.fillAmount <= 0.4f && Healthbarhealth.fillAmount > 0.1f) {
            Healthbarhealth.color = Color.yellow;
        }
        if (Healthbarhealth.fillAmount <= 0.15f) {
            Healthbarhealth.color = Color.red;
        }
    }

    private void UpdateHealthAmount() {
        healthAmount.text = playerHealthSystem.GetCurrentHealthAmount().ToString() + "/" + playerHealthSystem.GetMaxHealthAmount().ToString();
    }
}
