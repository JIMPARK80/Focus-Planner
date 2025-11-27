# Focus Planner v0.3

WPF 기반 할 일 관리 애플리케이션 (Front-End Only)

## 기능

- ✅ 할 일 추가
- ✅ 완료 체크
- ✅ 완료된 항목 삭제
- ✅ 목록 새로고침
- ✅ Enter 키로 빠른 추가
- ✅ 완료된 항목을 오른쪽에 별도로 표시

## 실행 방법

### 방법 1: 솔루션 파일 사용 (권장)

```bash
# 솔루션 빌드
dotnet build "Focus Planner.sln"

# 애플리케이션 실행
dotnet run --project FocusPlanner.csproj
```

### 방법 2: 프로젝트 파일 직접 지정

```bash
# 프로젝트 빌드
dotnet build FocusPlanner.csproj

# 애플리케이션 실행
dotnet run --project FocusPlanner.csproj
```

### 방법 3: Visual Studio

Visual Studio에서 `Focus Planner.sln` 파일을 열고 F5로 실행

## 기술 스택

- .NET 8.0
- WPF (Windows Presentation Foundation)
- Pure C# code-only UI (no XAML)

## 다음 단계

- Google Sheets API 연결
- 백엔드 통합

