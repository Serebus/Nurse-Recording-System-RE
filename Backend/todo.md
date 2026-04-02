# Logout 400 Fix - TODO Steps

## Plan Summary
Fix POST /api/Auth/logout 400 by ensuring cookie delivery (CORS/HTTPS), verify SPs, clean up services, test end-to-end.

## Steps (to be checked off as completed):

- [x] **Step 1**: Read and verify Stored Procedures file contents (Auth_and_session_storedprocedure.txt)
- [x] **Step 2**: Update CORS in Program.cs to include HTTPS frontend origins
- [x] **Step 3**: Update authStore.js logout fetch (remove body/Content-Type)
- [x] **Step 4**: Add logging to AuthController LogoutUser for debugging
- [ ] **Step 5**: Test login/logout via Swagger (manual cookie check)
- [ ] **Step 6**: Test full frontend flow (vite dev server)
- [ ] **Step 7**: Complete - run attempt_completion

**Current Progress: 4/7**

## Notes
- SPs good, table likely `SessionToken` (SPs use it).
- Logging added to pinpoint 400 (cookie missing/invalid).
- Test: 
  1. Backend: `cd Backend && dotnet run` → Swagger /swagger
  2. Login nurse → copy SessionToken cookie → POST /logout (Postman/Swagger) with cookie → check logs.
  3. Frontend: `cd Frontend/Web && npm run dev -- --https` → login → logout → check network/logs.
- If still 400, SP or table issue.

**Next manual steps for user: Run backend/frontend + test logout, share terminal/console logs if fails.**
