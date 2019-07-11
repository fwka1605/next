暫定 asp.net core Web API の利用方法

login をして、AccessToken を取得

```code
URL http://domain/api/login/login
Http Method: POST

custom header
Content-Type: application/json
VOne-AuthenticationKey: AAA
VOne-TenantCode: RAC990

body
{
  "companyCode":"0001",
  "userCode":"L005",
  "password":"z"
}
```

取得後は、各APIを利用可

```code
URL http://domain/api/accounttitlemaster/getbycode
Http Method:POST

custom header (access_token)
VOne-AccessToken: FFC37BAB-7ECF-4CA5-94F6-D080B17346CA

body
{
  "useCommonSearch":false,
  "companyId":1,
  "codes":[],
  "name":"",

}
```