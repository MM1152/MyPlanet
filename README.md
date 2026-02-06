<div align="center">
<h2>[2026] 버던트 프로토콜 🌍</h2>
<p>유니티 합반 프로젝트<br>
프리셋 기반 전략형 디펜스<br>
</p>
<img src="https://github.com/user-attachments/assets/9c1aac62-ccf9-4e3c-aa0a-43078c30f287" height="400"/>
</div>

---

### 🗂️ 개요

- **인원**: 개발 2인 , 기획 3인
- **프로젝트명**: 버던트 프로토콜
- **빌드**: Mobile ( Android )
- **개발툴**: Unity Engine
- **기간**: 2025-11 ~ 2026-01 ( 2달 )

---
🔗 [전체 Script 폴더 보기](https://github.com/MM1152/MyPlanet/tree/main/Assets/Script)

```
Assets/Script/
│
├── 📁 Animation/
├── 📁 Consumables/
├── 📁 Core/                          # 핵심 게임 시스템
│   ├── 📁 DataTable/
│   ├── 📁 DesignPatten/
│   ├── 📁 Firebase/                  # Firebase 통합
│   ├── 📁 Interface/
│   ├── 📁 Json/
│   ├── 📁 Loading/
│   ├── 📁 Manager/
│   ├── 📁 Popup/
│   ├── 📁 Sound/
│   ├── 📁 StatusEffect/
│   ├── 📁 Tower/
│   ├── 📁 UI/
│   └──📁 Window/
├── 📁 DataTable/
├── 📁 Debug/
├── 📁 DefenceTower/                  # 방어 타워 시스템
│   ├── 📁 Tower/                     # 타워 클래스들
│   └── 📁 TowerAttackPrefabs/        # 타워 공격 프리팹
├── 📁 Enemy/                         # 적 AI 및 로직
├── 📁 Event/
├── 📁 Particle/
├── 📁 PickUp/
├── 📁 Planet/
├── 📁 Scene/
├── 📁 Stage/
├── 📁 StatusEffect/
├── 📁 Terraforming/
├── 📁 TestCodes/
├── 📁 TextUI/
├── 📁 TitleScene/                    # 타이틀 화면 관련
├── 📁 Tutorial/                      # 튜토리얼 시스템
│   ├── 📁 Book/                      # 도감 튜토리얼
│   ├── 📁 Stage1/                    # 스테이지 1 튜토리얼
│   └── 📁 Stage1Enter/               # 스테이지 진입 튜토리얼
├── 📁 UI/                            # UI 컴포넌트
└── 📁 Wave/                          # 웨이브 시스템
```

## 🔑 주요 코드 파일

### 📚 Tutorial 관련

| 파일 | 설명 | 링크 |
|------|------|------|
| `TutorialManager.cs` | 튜토리얼 전체 관리 및 진행 제어 | [📄 보기](https://github.com/MM1152/MyPlanet/blob/main/Assets/Script/Tutorial/TutorialManager.cs) |
| `Tutorial.cs` | 튜토리얼 베이스 클래스 | [📄 보기](https://github.com/MM1152/MyPlanet/blob/main/Assets/Script/Tutorial/Tutorial.cs) |
| `Tutorial 폴더` | 전체 튜토리얼 스크립트 | [📁 보기](https://github.com/MM1152/MyPlanet/tree/main/Assets/Script/Tutorial) |

### 🔥 Firebase 관련

| 파일 | 설명 | 링크 |
|------|------|------|
| `FirebaseManager.cs` | Firebase 싱글톤 매니저 (Auth, DB, UserData 통합) | [📄 보기](https://github.com/MM1152/MyPlanet/blob/main/Assets/Script/Core/Firebase/FirebaseManager.cs) |
| `Firebase 폴더` | 전체 Firebase 관련 코드 | [📁 보기](https://github.com/MM1152/MyPlanet/tree/main/Assets/Script/Core/Firebase) |

### 🗼 Tower 관련

| 파일 | 설명 | 링크 |
|------|------|------|
| `Tower.cs` | 타워 베이스 클래스 (공격, 스탯, 옵션) | [📄 보기](https://github.com/MM1152/MyPlanet/blob/main/Assets/Script/DefenceTower/Tower/Tower.cs) |
| `DefenceTower 폴더` | 전체 타워 관련 코드 | [📁 보기](https://github.com/MM1152/MyPlanet/tree/main/Assets/Script/DefenceTower) |

### 🎯 Core 시스템

| 파일 | 설명 | 링크 |
|------|------|------|
| `Define.cs` | 게임 상수, Enum 정의 | [📄 보기](https://github.com/MM1152/MyPlanet/blob/main/Assets/Script/Core/Define.cs) |
| `TypeEffectiveness.cs` | 속성 상성 계산 | [📄 보기](https://github.com/MM1152/MyPlanet/blob/main/Assets/Script/Core/TypeEffectiveness.cs) |
| `Utils.cs` | 유틸리티 함수 모음 | [📄 보기](https://github.com/MM1152/MyPlanet/blob/main/Assets/Script/Core/Utils.cs) |

---

### 🎮 게임소개

#### 🌿 **프리셋**
- 프리셋 기능으로 자신의 전략을 설계하세요!  
- 행성과 타워를 자유롭게 조합, 타워의 고유 옵션을 원하는 위치에 배치해 최적의 효과를 누릴 수 있습니다.

#### 🪐 **행성**
- **15가지 개성 넘치는 행성!**
- 각 행성별 특수한 패시브 능력 제공  
- **뽑기로 행성 획득 & 성급 강화**  
- 플레이를 통해 재화를 모아 행성을 강화하며 더욱 높은 스테이지에 도전하세요!

#### 🏰 **타워**
- 총 **16종 공격타워**와 **8종 유틸타워**로 다양한 전략 완성!
- 뽑기・상점에서 새로운 타워 획득  
- 타워를 강화하여 적을 압도하세요  
- 프리셋에 등록한 타워들을 전장에서 활용해 더 높은 웨이브에 도전하세요!

#### 👾 **적**
- 상성 시스템으로 전략적 전투!
- 웨이브마다 등장하는 엘리트 몬스터를 처치해 타워 버프를 획득하세요  
- 각 스테이지에서 개성 넘치는 보스와 대결, 치밀한 공략이 필요합니다!

---

<table align="center">
<tr>
<td align="center" width="50%">
   <img width="271" height="557" alt="Image" src="https://github.com/user-attachments/assets/e3211542-b4bf-4848-96c3-87b2c0980ce6" />
  <br>
  <sub>게임 메인화면/플레이 예시</sub>
</td>
<td align="center" width="50%">
 <img width="271" height="557" alt="Image" src="https://github.com/user-attachments/assets/7c6c1553-2d90-4fd4-aa99-0155c38b5259" />
  <br>
  <sub>통합 테스트 환경 예시</sub>
</td>
</tr>
</table>

#### ★ 구현 파트
<p>
<table align="center" width="100%">
  <tr>
    <td align="top" width="600">
      <b>개발 팀장 (천민성)</b><br><br>
        - Firebase 로그인, 데이터베이스 관리<br>
        - 게임 데이터 관리<br>
        - 전투 시스템 (타워, 행성) 구현<br>
        - 프리셋 기능 구현<br>
        - 상점, 뽑기 시스템 구현<br>
        - 통합 테스트 환경 구축
    </td>
    <td align="top" width="600">
      <b>개발 팀원 (이재영)</b><br><br><br>
        - 전투 시스템 (적) 구현<br>
        - 웨이브 기반 스폰 시스템 구현<br>
        - 데이터 비동기 로드 작업<br>
        - 오브젝트 풀링<br>
       <br>
    </td>
  </tr>
</table>
</p>

---

### 🏗️ 구현 화면 예시


<table align="center">
<tr>
  
<td align="center"><img src="https://github.com/user-attachments/assets/e3e65e5f-d1d5-4d5a-b5de-a8d6dd8546cd" height="400"/><br><sub>뽑기 시스템</sub></td>
<td align="center"><img src="https://github.com/user-attachments/assets/88579604-0dfd-4e27-91f6-38eee41550fa" height="400"/><br><sub>프리셋</sub></td>
</tr>
<tr>
<td align="center"><img src="https://github.com/user-attachments/assets/45af91bb-6e50-41ce-af32-0c0279492452" height="400"/><br><sub>인게임 플레이</sub></td>
<td align="center"><img src="https://github.com/user-attachments/assets/63349481-6939-4a62-9385-20c1ee1e5723" height="400"/><br><sub>보스 전투/비동기 레이드</sub></td>
</tr>
</table>
