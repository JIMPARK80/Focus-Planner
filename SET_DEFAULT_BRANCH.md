# GitHub에서 main을 기본 브랜치로 설정하기

## 방법 1: GitHub 웹 인터페이스 (가장 쉬움) ⭐

1. **GitHub 저장소 페이지로 이동**
   - https://github.com/JIMPARK80/Focus-Planner 접속

2. **Settings로 이동**
   - 저장소 페이지 오른쪽 상단의 **Settings** 탭 클릭

3. **Branches 메뉴 선택**
   - 왼쪽 사이드바에서 **Branches** 클릭

4. **Default branch 변경**
   - **Default branch** 섹션에서 현재 `master` 옆의 스위치/연필 아이콘 클릭
   - 드롭다운에서 `main` 선택
   - **Update** 버튼 클릭
   - 확인 팝업에서 "I understand, update the default branch" 클릭

## 방법 2: GitHub CLI 사용 (선택사항)

```bash
gh api repos/JIMPARK80/Focus-Planner --method PATCH -f default_branch=main
```

## 로컬 저장소 업데이트 (기본 브랜치 변경 후)

기본 브랜치를 변경한 후, 로컬에서 원격 HEAD 참조를 업데이트하세요:

```bash
git fetch origin
git remote set-head origin -a
```

또는 직접 설정:
```bash
git symbolic-ref refs/remotes/origin/HEAD refs/remotes/origin/main
```
