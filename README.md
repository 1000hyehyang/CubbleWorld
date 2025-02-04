# CubbleWorld

## 📚 프로젝트 개요  
Unity를 활용해 **간단한 창작물을 실시간으로 제작하고 공유**할 수 있는 미니 마인 큐브 크래프트입니다. 이 프로젝트는 UI 심화 기능부터 네트워킹, 직렬화 등 다양한 기능을 구현하며, Unity 개발 환경에 대한 이해도를 높이기 위해 설계되었습니다.  

<br>

## 🛠️ 구현 기능  

### **1. Unity UI 심화 기능**
- 레벨 에디터 기능 설계  
- 공장 패턴(Factory Pattern)과 **Prefab 카탈로그 UI**  
- UI 레이아웃 구조 최적화 (`min/preferred`, `ExecuteAlways`)  
- **RenderTexture**와 **Shader**를 활용한 실시간 렌더링 효과  
- TextMeshPro(TMP) 및 캔버스 최적화  

### **2. 직렬화(Serialization) 기능**
- **동적 데이터**와 **정적 데이터**의 관리 구조  
- **ScriptableObject(SO)**를 활용한 데이터 관리  
- SO List 패턴 구현 및 **Prefab과의 연동**  
- **JSON**을 통한 데이터 직렬화 및 입출력(IO) 기능  

### **3. 네트워킹 및 멀티플레이어 기능**
- **Photon Fusion**을 활용한 네트워크 멀티플레이어 구현  
- **NetworkObject** 및 **NetworkTransform** 동기화  
- RPC(Remote Procedure Call)를 활용한 **동기화 최적화**  
- 네트워크 직렬화 및 동기화 데이터 처리  

### **4. Unity Editor 스크립트**
- Custom **Editor Script**를 통한 작업 자동화  
- **MenuItem**을 활용한 편리한 에디터 확장  
- ScriptableObject, Prefab, 어셋 자동 관리  
- **CustomEditor** 및 **PropertyDrawer** 구현  
- 에디터 전용 창(Window) UI 개발  

<br>

## 💡 사용 기술 스택
- **Unity** (최신 버전 권장)  
- **Photon Fusion** (네트워킹)  
- **C#** (Unity 스크립팅)  
- **TextMeshPro(TMP)**  
- **Shader Graph** 및 **RenderTexture**  
- **JSON** 직렬화 및 파일 IO  

<br>

## 🤝 기여 방법  
1. 프로젝트를 포크 후 새로운 브랜치를 생성합니다.  
   ```bash
   git checkout -b feature/새로운기능
   ```  
2. 변경 사항을 커밋하고 푸시합니다.  
   ```bash
   git commit -m "기능 추가: 설명 작성"
   git push origin feature/새로운기능
   ```  
3. Pull Request(PR)를 생성하여 리뷰를 요청합니다.  

<br>

## 📄 라이선스  
이 프로젝트는 **MIT 라이선스**를 따릅니다. 자세한 내용은 `LICENSE` 파일을 참고하세요.  
