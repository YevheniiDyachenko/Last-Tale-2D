Звісно\! Проаналізувавши ваші документи та зіставивши їх із підходами з книги "Розробка ігор на Unity - 2025" від Борромео та Гоміли Салас, я підготував єдиний, покращений Game Design Document (GDD) з удосконаленою структурою проєкту та скриптами.

Основна ідея полягала в тому, щоб об'єднати креативні концепції з ваших GDD і застосувати до них професійні, оптимізовані практики, описані в книзі. Це стосується як організації файлів у проєкті, так і написання чистого, масштабованого коду на C\#.

Нижче наведено фінальний GDD та покроковий туторіал для створення прототипу.

-----

### Game Design Document (GDD): Остання казка: Полювання на Колобка

#### **1. Концепт**

  * **Назва:** Остання казка: Полювання на Колобка.
  * [cite\_start]**Жанр:** Top-Down Co-op Boss Fight з елементами roguelite. [cite: 1749, 1996]
  * [cite\_start]**Гравці:** 1–2 (кооператив; у соло-режимі — AI-компаньйон для другого персонажа). [cite: 1750, 1997]
  * [cite\_start]**Ключова ідея:** Гравці — лісові звірі, які мають підготуватися й знищити головного ворога — Колобка, невмирущого казкового монстра, що пожирає все на своєму шляху. [cite: 1751, 1998]
  * [cite\_start]**Elevator Pitch:** "У магічному лісі звірі об'єднуються, щоб знищити невмирущого Колобка — суміш хаосу Overcooked, бос-файтів Dark Souls і казкового хорору з чорним гумором". [cite: 1999]
  * [cite\_start]**Цільова аудиторія:** Фанати кооперативних ігор (It Takes Two, Hades, Dead Cells), які люблять швидкі сесії (5-10 хв), з акцентом на командну роботу, рандомізацію та фольклорні теми з темним відтінком. [cite: 2000]

-----

#### **2. Наратив і сетинг**

  * [cite\_start]**Сетинг:** Магічний ліс, натхненний слов'янським фольклором. [cite: 1753, 2002] [cite\_start]Ліс — це динамічна арена з густими хащами, ріками та руїнами, де Колобок є втіленням голоду й руйнування. [cite: 2003]
  * [cite\_start]**Тон:** Суміш чорного гумору та казкового хорору. [cite: 1754, 2004]
  * **Наративний цикл:**
    1.  [cite\_start]**Легенда:** Колобок — стародавнє прокляття, що пожирає все живе. [cite: 1756, 2006]
    2.  [cite\_start]**Підготовка:** Два герої (лисиця та вовк/заєць) збирають сили, ресурси та пастки. [cite: 1757, 2007]
    3.  [cite\_start]**З'явлення:** Колобок з'являється в центрі карти після таймера. [cite: 1758, 2008]
    4.  [cite\_start]**Фінал:** Перемога — Колобок знищений, ліс врятований. [cite: 2009] [cite\_start]Поразка — Колобок тікає, накладаючи "прокляття" (debuff на наступний раунд). [cite: 1759, 2010]
  * [cite\_start]**Метанаратив:** З кожним раундом ліс "еволюціонує", а історія "останньої казки" завершується після 10 перемог. [cite: 2012]

-----

#### **3. Основна петля геймплею**

1.  [cite\_start]**Підготовка (60–90 сек., таймер на екрані):** [cite: 1761, 2014]
      * [cite\_start]Збір ресурсів (ягоди, гілки). [cite: 1762, 2015]
      * [cite\_start]Прокачування здібностей (вибір з 2-3 опцій). [cite: 2016]
      * [cite\_start]Розстановка пасток (ями, капкани; обмежена кількість). [cite: 1763, 2017]
2.  [cite\_start]**Бос-фаза (2-5 хв):** [cite: 1764, 2019]
      * [cite\_start]Спавн Колобка в центрі. [cite: 1765, 2020]
      * [cite\_start]Кілька стадій розвитку (катається → стрибає → руйнує пастки). [cite: 1766, 2021]
      * [cite\_start]Бій: Командна взаємодія — один відволікає, інший веде в пастку. [cite: 1768, 2022]
3.  **Фінал:**
      * [cite\_start]**Перемога:** Колобок втрачає все HP. [cite: 1771, 2027]
      * [cite\_start]**Поразка:** Колобок тікає або гравці вмирають. [cite: 1772, 2028]

-----

#### **4. Механіки**

  * **Персонажі-ролі:**
      * [cite\_start]**Лисиця (хитрість, пастки):** Stealth Mode, Trap Boost. [cite: 1775, 2032]
      * [cite\_start]**Вовк (сила, ближній бій):** Charge Attack, Roar. [cite: 1776, 2034]
      * [cite\_start]**Заєць (швидкість, агро):** Dodge Roll, Taunt. [cite: 1777, 2036]
  * **Колобок (бос):**
      * [cite\_start]**1 стадія:** Катається, таранить. [cite: 1779, 2039]
      * [cite\_start]**2 стадія:** Стрибки, удари по землі (AoE). [cite: 1780, 2040]
      * [cite\_start]**3 стадія:** Швидкий, імунний до базових пасток. [cite: 1781, 2041]
  * **Прогресія (після перемог):**
      * [cite\_start]Нові пастки, апгрейд абілок, нові арени. [cite: 1783, 2042]

-----

#### **5. Баланс і системний дизайн**

  * [cite\_start]**Ресурси:** Час (обмежений), пастки (витратні), енергія для абілок. [cite: 1785, 2048]
  * [cite\_start]**Взаємодія:** Пастки + кооперативна синергія (комбо-атаки). [cite: 1786, 2052]
  * [cite\_start]**Зворотний зв’язок:** Складність Колобка зростає з часом бою (rage mode). [cite: 1787, 2053]
  * **Формули балансу:**
      * [cite\_start]**Damage scaling:** `Базовий удар = 20 + (рівень * 5)`. [cite: 2056]
      * [cite\_start]**HP Колобка:** `1000 + (раунд * 200)`. [cite: 2057]

-----

### 📖 Покроковий туторіал створення прототипу

Цей туторіал розроблено в стилі книги "Розробка ігор на Unity - 2025", з акцентом на чистоту коду, правильну структуру та використання основних можливостей Unity.

#### **Крок 1: Створення проєкту та налаштування сцени**

1.  [cite\_start]**Створіть новий 2D проєкт в Unity Hub.** Назвіть його `LastTale_Kolobok`. [cite: 1932, 1933, 1934]
2.  [cite\_start]**Створіть головну сцену.** Збережіть її як `Arena.unity` у папці `Assets/Scenes`. [cite: 1935]
3.  **Налаштуйте арену:**
      * Додайте `Sprite -> Square` і розтягніть його, щоб створити підлогу. Назвіть об'єкт `Ground`.
      * [cite\_start]Створіть порожній об'єкт `GameManager` для керування грою. [cite: 1939]
      * [cite\_start]Створіть порожній об'єкт `CenterSpawnPoint` для появи Колобка. [cite: 1938]

-----

#### **Крок 2: Створення гравця та реалізація руху**

1.  **Створіть гравця:**
      * [cite\_start]Додайте `Sprite -> Capsule` і назвіть його `Player`. [cite: 1792, 1941]
      * [cite\_start]Додайте компоненти `Rigidbody 2D` (Body Type: Dynamic, Freeze Rotation Z) та `CapsuleCollider2D`. [cite: 1943, 1944]
      * Призначте йому тег `Player`.
2.  **Скрипт руху:**
      * Створіть скрипт `PlayerController.cs` у `Assets/Scripts/Player/`.
      * **Покращення з книги:** Замість `MovePosition` у `FixedUpdate`, який може викликати проблеми з фізикою, будемо використовувати `Rigidbody2D.velocity` в `Update`. [cite\_start]Це забезпечує більш плавний та передбачуваний рух, як рекомендується для динамічних об'єктів. [cite: 194]

<!-- end list -->

```csharp
// Scripts/Player/PlayerController.cs
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movementInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Отримуємо введення гравця
        movementInput.x = Input.GetAxisRaw("Horizontal");
        movementInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        // Застосовуємо швидкість до Rigidbody. Це краще для фізики.
        rb.velocity = movementInput.normalized * moveSpeed;
    }
}
```

-----

#### **Крок 3: Створення боса "Колобок"**

1.  **Створіть Колобка:**
      * [cite\_start]Додайте `Sprite -> Circle` і назвіть його `Kolobok`. [cite: 1790, 1953]
      * [cite\_start]Додайте `Rigidbody 2D` (Dynamic) та `CircleCollider2D`. [cite: 1954, 1955]
      * Призначте тег `Boss`.
2.  **Скрипт Колобка:**
      * Створіть скрипт `KolobokController.cs` у `Assets/Scripts/Boss/`.
      * **Покращення з книги:** Код буде більш структурованим, з окремими методами для логіки. [cite\_start]Додамо систему здоров'я та стадій. [cite: 411]

<!-- end list -->

```csharp
// Scripts/Boss/KolobokController.cs
using UnityEngine;

public class KolobokController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private int maxHealth = 1000;
    
    private int currentHealth;
    private int stage = 1;
    private Transform target;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        // Знаходимо гравця по тегу - це більш надійно
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    void FixedUpdate()
    {
        if (target == null) return;
        
        // Рух до цілі через фізику
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * moveSpeed;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Kolobok Health: {currentHealth}");
        if (currentHealth <= 0)
        {
            Die();
        }
        // Перевірка переходу на наступну стадію
        CheckStageTransition();
    }

    private void CheckStageTransition()
    {
        if (stage == 1 && currentHealth < maxHealth * 0.7f)
        {
            NextStage();
        }
        else if (stage == 2 && currentHealth < maxHealth * 0.3f)
        {
            NextStage();
        }
    }

    public void NextStage()
    {
        stage++;
        moveSpeed *= 1.5f; // Стає швидшим
        Debug.Log($"Kolobok entered stage {stage}!");
    }

    private void Die()
    {
        Debug.Log("Kolobok is defeated!");
        // Тут буде логіка перемоги
        Destroy(gameObject);
    }
}
```

-----

#### **Крок 4: Система пасток**

1.  **Створіть префаб пастки:**
      * Створіть `Sprite -> Square`, назвіть `Trap`.
      * [cite\_start]Додайте `BoxCollider2D` з увімкненим `Is Trigger`. [cite: 1969]
2.  **Скрипт пастки:**
      * Створіть скрипт `Trap.cs` у `Assets/Scripts/Systems/`.
      * **Покращення з книги:** Код буде чистішим, з перевіркою наявності компонента `KolobokController` перед викликом його методів. [cite\_start]Це запобігає помилкам. [cite: 189]

<!-- end list -->

```csharp
// Scripts/Systems/Trap.cs
using UnityEngine;

public class Trap : MonoBehaviour
{
    [SerializeField] private int damage = 50;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Boss"))
        {
            KolobokController boss = other.GetComponent<KolobokController>();
            if (boss != null)
            {
                boss.TakeDamage(damage);
                Debug.Log("Kolobok hit a trap!");
            }
            // Знищуємо пастку після використання
            Destroy(gameObject);
        }
    }
}
```

-----

#### **Крок 5: GameManager та ігровий цикл**

1.  **Створіть скрипт `GameManager.cs`** у `Assets/Scripts/Managers/`.
2.  **Прикріпіть його до об'єкта `GameManager`.**
3.  [cite\_start]**Покращення з книги:** Використаємо корутину (`Coroutine`) для таймера, як це рекомендовано для часових затримок, замість перевірки в `Update`. [cite: 58] Це більш ефективно.

<!-- end list -->

```csharp
// Scripts/Managers/GameManager.cs
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject kolobokPrefab;
    [SerializeField] private Transform centerSpawnPoint;
    [SerializeField] private float preparationTime = 60f;

    void Start()
    {
        StartCoroutine(PreparationPhase());
    }

    private IEnumerator PreparationPhase()
    {
        Debug.Log("Preparation phase has started. You have " + preparationTime + " seconds.");
        yield return new WaitForSeconds(preparationTime);
        SpawnBoss();
    }

    private void SpawnBoss()
    {
        Debug.Log("Kolobok has spawned!");
        Instantiate(kolobokPrefab, centerSpawnPoint.position, Quaternion.identity);
    }
}
```

-----

#### **6. Збірка та тестування прототипу**

1.  **Створіть префаби:** Перетягніть об'єкти `Player`, `Kolobok` та `Trap` з ієрархії у папку `Assets/Prefabs`.
2.  **Налаштуйте `GameManager`:** Перетягніть префаб `Kolobok` у відповідне поле скрипта `GameManager`.
3.  **Додайте можливість ставити пастки:** Створіть простий скрипт `TrapPlacer.cs` для гравця, який буде створювати екземпляр префабу пастки при натисканні клавіші 'E'.
4.  **Додайте атаку гравця:** Створіть скрипт `PlayerAttack.cs`, який при натисканні 'Space' завдає шкоди Колобку, якщо той знаходиться в зоні досяжності.
