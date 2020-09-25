
[![build-and-test Actions Status](https://github.com/JJhuk/Server/workflows/build-and-test/badge.svg)](https://github.com/JJhuk/Server/actions)

### 서버

- .NET Core 3.1 & C# 8.0 기능 적극 사용
- C# 프로젝트 구조
    - API 프로젝트 (ASP.NET Core)
    - Domain 프로젝트 (EntityFramework)
    - Test 프로젝트 (Xunit)
    - Direcctory.Build.Props (버전 및 공통 디펜던시 관리)
        - [https://docs.microsoft.com/en-us/visualstudio/msbuild/customize-your-build?view=vs-2019](https://docs.microsoft.com/en-us/visualstudio/msbuild/customize-your-build?view=vs-2019)
- 유용한 라이브러리
    - [https://github.com/morelinq/MoreLINQ](https://github.com/morelinq/MoreLINQ)
    - [https://automapper.org/](https://automapper.org/)

- [ASP.NET](http://asp.NET) Core
    - [ ]  Access / Refresh 토큰을 통해서 인증이 되어야함
        - 이거 사용하면 좀 쉽게 구현 가능 :  [https://github.com/WDWWW/aspnetcore-token-authentication](https://github.com/WDWWW/aspnetcore-token-authentication)
    - [x]  `[Authorize]` Attribute를 사용하여 Action Method를 구현해야함
    - [ ]  Layered Architecture : Controller / Service(Optional) / Repository
        - [https://medium.com/@priyalwalpita/software-architecture-patterns-layered-architecture-a3b89b71a057](https://medium.com/@priyalwalpita/software-architecture-patterns-layered-architecture-a3b89b71a057)
    - [x]  *Request / *Response / Entity(EFCore의 테이블 클래스) 분리하여 작성할 것
    - [x]  Attribute Model Validation 이용할 것
    - [x]  Swagger 생성할 것
    - [ ]  환경별 로거 설정할 것
    - API 규칙은 Restful하게 작성할 것(추천 글은 아래 참조)
        - [https://meetup.toast.com/posts/92](https://meetup.toast.com/posts/92)
        - [https://gmlwjd9405.github.io/2018/09/21/rest-and-restful.html](https://gmlwjd9405.github.io/2018/09/21/rest-and-restful.html)
        - [https://docs.microsoft.com/ko-kr/azure/architecture/best-practices/api-design#use-hateoas-to-enable-navigation-to-related-resources](https://docs.microsoft.com/ko-kr/azure/architecture/best-practices/api-design#use-hateoas-to-enable-navigation-to-related-resources)
        - 그렇다고 HATEOS의 모든 원칙을 따를 필요는 없음
- XUnit
    - [x]  헬퍼메서드나 클래스에 대한 유닛 테스트 작성할 것
    - [x]  GWT Pattern 사용 : [https://velog.io/@pop8682/번역-Given-When-Then-martin-fowler](https://velog.io/@pop8682/%EB%B2%88%EC%97%AD-Given-When-Then-martin-fowler)
    - 필요할 시 Mock 라이브러리 사용 : [https://nsubstitute.github.io/](https://nsubstitute.github.io/)
    - [ ]  API에 대한 Integration 테스트 작성하기
- Entity Framework Core : Database에 대한 ORM 프레임워크
    - [x]  Migration 기능을 이용하여 테이블을 점진적으로 생성/변경해나가기
    - [x]  Development 환경에서만 Sensitive Logging 활성화 할 것
    - [x]  Lazy Proxy 활성화할 것
    - [ ]  모든 엔티티에 대해서 CreatedAt, UpdatedAt을 구현하고 이를 DbContext의 Hooking하여 자동적으로 업데이트 되도록 할 것
    - [x]  모든 엔티티에 대해서 IsDeleted 프로퍼티가 구현어야함
        - [x]  Global Filter를 적용하여 기본 쿼리 결과에 대해서 IsDeleted True일 때 제외시킬 것
        - [ ]  DbContext.Remove()함수 호출시 실제로 삭제시키는 것이 아닌 IsDeleted를 True로 만들어 줄 것
    - Hooking 방식에 대해서는 Override를 하든 라이브러리를 쓰든 무방 = 라이브러리 사용시 반드시 원리에 대해서 인지하고 사용할 것
- [x]  Datebase : Postgresql

---

### Devops

- [x]  Github Actions를 통해서 커밋별 Build / Test 실행할 것
- [x]  [ASP.NET](http://asp.NET) Core를 Dockerfile로 만들어서 Github Packages에 배포할 것
