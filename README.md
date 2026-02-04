# 🎭 Love Marionette: AI 로맨스 스캠 시뮬레이터 (Demo)

![Unity](https://img.shields.io/badge/Unity-6000.2.11f1-black?style=flat&logo=unity)
![Python](https://img.shields.io/badge/Python-3.10-blue?style=flat&logo=python)
![FastAPI](https://img.shields.io/badge/Backend-FastAPI-009688?style=flat&logo=fastapi)
![AI](https://img.shields.io/badge/AI-Groq%20(Llama3)-orange?style=flat)

> **"당신의 운명적인 사랑, 진짜일까요? 아니면 조종당하는 꼭두각시일까요?"**
> 생성형 AI(LLM)를 활용한 실시간 대화형 로맨스 스캠 예방 시뮬레이션 게임입니다.

<br/>

## 📸 Screenshots (일부 화면)
<br/>
<img width="2108" height="1185" alt="image" src="https://github.com/user-attachments/assets/b9c86bef-d157-4a45-aaf8-ae4c1134f18e" />
<img width="2185" height="1251" alt="image" src="https://github.com/user-attachments/assets/a3d4e47d-86b4-4a90-8c36-0ec63fe900da" />
<img width="1800" height="1167" alt="image" src="https://github.com/user-attachments/assets/417f7ece-23dd-4dc9-bf53-6f1f248d08a0" />
<img width="2168" height="1206" alt="image" src="https://github.com/user-attachments/assets/b6ccee34-ff7e-4fec-808a-2d5a488d6259" />


## 📝 프로젝트 소개 (Introduction)
이 프로젝트는 **최신 생성형 AI(Llama-3)**를 활용하여, 실제 로맨스 스캠 범죄자들의 패턴을 학습한 NPC '에로스'와 대화하며 스캠 범죄를 식별하고 대처하는 능력을 기르는 **교육용 시뮬레이션 게임**입니다.
현 프로젝트는 완전 버전이 아닌 현재 Demo 버전입니다.

플레이어는 가상의 데이팅 앱 상황 속에서 AI와 실시간으로 대화하며, 호감을 쌓으려는 시도와 금전 요구의 압박 속에서 올바른 선택을 해야 합니다.

<br/>

## ✨ 핵심 기능 (Key Features)

### 🤖 LLM 기반의 지능형 사기꾼 NPC
- **실시간 대화 생성:** 정해진 스크립트가 아닌, 플레이어의 반응에 따라 유동적으로 대화하는 AI.
- **심리 전술 구현:** 라포(Rapport) 형성, 미래 암시, 가스라이팅, 감정 호소 등 실제 스캠 범죄자의 대화 패턴 모사.

### 🎮 Unity 3D 인터랙티브 환경
- **시네마틱 연출:** 뉴스 보도 형식의 인트로/아웃트로 및 타자기 효과(Typewriter Effect)를 적용한 UI.

### 🚦 멀티 엔딩 시스템
- **Bad Ending (피해 발생):** 사기꾼의 금전 요구에 응하거나 개인정보를 넘겨준 경우.
- **Normal Ending:** 사기임을 눈치챘지만 미온적으로 대처한 경우.
- **Clear Ending (예방 성공):** 논리적으로 사기꾼을 압박하여 스스로 포기하게 만들거나 신고한 경우.
- **AI 결과 리포트:** 게임 종료 후, LLM이 플레이어의 대화 내용을 분석하여 구체적인 피드백과 조언을 제공.

<br/>

## 🛠 기술 스택 (Tech Stack)

| 구분 | 기술 | 설명 |
| --- | --- | --- |
| **Game Engine** | Unity 6 (6000.2.11f1) | C# 스크립팅, UI/UX, 물리 엔진 |
| **Backend Server** | Python (FastAPI) | AI 통신 중계, 게임 로직 처리 |
| **AI Model** | Groq API (Llama-3.1-8b) | 초고속 추론 및 자연어 생성 |
| **Build Tool** | PyInstaller | Python 서버 실행 파일(.exe) 패키징 |

<br/>

## 🚀 설치 및 실행 방법 (How to Play)

이 게임은 **Unity 클라이언트**와 **Python 로컬 서버**가 함께 실행되어야 합니다.

### 1. 사전 준비 (Prerequisites)
- [Groq API Key](https://console.groq.com/) 발급이 필요합니다.

### 2. 서버 실행 (Python)
```bash
# 1. 저장소 클론
git clone https://github.com/Love-Marionette/Love-Marionette.git

# 2. 필수 라이브러리 설치
pip install fastapi uvicorn groq pydantic python-multipart

# 3. API 키 설정 (main.py 내부 수정)
# main.py 파일의 GROQ_API_KEY 변수에 본인의 키를 입력하세요.

# 4. 서버 실행
python main.py
```

### 3. 게임 실행 (Unity)
1. 서버가 켜진 상태(`http://127.0.0.1:8000`)에서 Unity 에디터를 실행하거나 빌드된 `Game.exe`를 실행합니다.
2. (배포판의 경우) 반드시 `main.exe`를 먼저 실행해주세요.

<br/>

## 📂 폴더 구조 (Directory Structure)
```text
📦 Love-Marionette
├── 📂 Assets               # Unity 프로젝트 에셋 (Scripts, Prefabs, Scenes)
│   ├── 📂 Scripts          # C# 게임 로직 (APIManager, ChatController 등)
│   └── ...
├── 📄 main.py              # FastAPI 서버 및 AI 로직 (핵심 코드)
├── 📄 main.spec            # PyInstaller 빌드 설정
├── 📄 README.md            # 프로젝트 설명서
└── 📄 .gitignore           # Git 제외 파일 목록
```

## 👨‍💻 참여자 (Contributors)
- 3인 참여
- @ty7766 : Unity 시뮬레이터 개발, AI 연동
- 다른 참여자들 : 기획, 디자인 요소 수집, AI 프롬프트 엔지니어링
---
*(Copyright © 2026. All rights reserved.)*
