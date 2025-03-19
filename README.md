# HTTPserver 문서

## 1. 개요
ASP.NET Core Web API 다양한 기능을 포함하여 확장성을 고려한 설계를 적용합니다.

## 2. 시스템 구성
- **클라이언트:** Postman, k6
- **서버:** ASP.NET Core Web(Restful) API
- **DB:** SQLServer (Entity Framework 사용), Redis

## 3. 아키텍처 다이어그램
```plaintext
+------------+        +----------------+        +------------+
|   Client   | <-->   |   API Server   | <-->   |     DB     |
+------------+        +----------------+        +------------+
```

## 4. 주요 기능
### 4.1. 네트워크 통신
- HTTP 기반 네트워크 통신 서버 구축
- `async/await` 활용하여 비동기 작업 구현

### 4.2. 로그인, 유저, 랭킹 등 컨텐츠 구현
- 로그인 인증 로직 구현 (JWT)
- 유저 정보 저장 로직 구현
- 랭킹 시스템 로직 구현

### 4.3. DTO 구조 설계
- 보안에 민감한 정보 노출을 방지하기 위해 필요한 데이터만 전달하도록 설계

### 4.4. 데이터베이스 연동
- Entity Framework Core를 활용한 데이터 저장 및 활용
- 사용자 및 게임 데이터 관리

### 4.5. k6 부하 테스트 진행 (Redis)
- 랭킹 로직 vus: 200 진행
- Redis 캐시 적용 후 진행

## 5. 개발 환경
- **IDE:** Visual Studio 2022
- **Framework:** .NET 8.0
- **Database:** SQLServer
- **ORM:** Entity Framework Core
- **Cache:** Redis
