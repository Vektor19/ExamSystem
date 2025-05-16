import Home from "./Home/Home.tsx";
import { Navigate, Route, Routes } from "react-router-dom";
import PublicLayout from "./Layouts/PublicLayout.tsx";
import PrivateRoute from "./Routes/PrivateRoute.tsx";
import DashboardLayout from "./Layouts/DashboardLayout.tsx";
import Dashboard from "./Dashboard/Dashboard.tsx";
import { AuthProvider } from "../Providers/AuthProvider.tsx";
import RegisterPage from "./AuthPages/RegisterPage.tsx";
import LoginPage from "./AuthPages/LoginPage.tsx";
import AuthRoute from "./Routes/AuthRoute.tsx";
import LogoutRoute from "./Routes/LogoutRoute.tsx";
import { UserProvider } from "../Providers/UserProvider.tsx";
import UserGuardRoute from "./Routes/UserGuardRoute.tsx";
import Profile from "./Dashboard/Profile.tsx";
import { ExamsProvider } from "../Providers/ExamsProvider.tsx";
import Exams from "./Dashboard/Exams.tsx";
import CreateExamPage from "./Dashboard/CreateExamPage.tsx";
import ExamEditPage from "./Dashboard/ExamEditPage.tsx";
import { ExamSessionProvider } from "../Providers/ExamSessionProvider.tsx";
import ExamSession from "./Dashboard/ExamSession.tsx";
import { AntiCheatingProvider } from "../Providers/AntiCheatingProvider.tsx";
import { NotificationProvider } from "../Providers/NotificationProvider.tsx";
import StudentExamResult from "./Dashboard/StudentExamResult.tsx";
import ParticipantAssessPage from "./Dashboard/ParticipantAssessPage.tsx";
import ParticipantViolationsPage from "./Dashboard/ParticipantViolationsPage.tsx";
import ExamsGuardRoute from "./Routes/ExamsGuardRoute.tsx";

const App = () => {
  return (
    <>
      <NotificationProvider>
        <AuthProvider>
          <UserProvider>
            <ExamsProvider>
              <Routes>
                <Route element={<PublicLayout />}>
                  <Route path="/" element={<Home />} />
                  <Route
                    path="/login"
                    element={
                      <AuthRoute>
                        <LoginPage />
                      </AuthRoute>
                    }
                  />
                  <Route
                    path="/register"
                    element={
                      <AuthRoute>
                        <RegisterPage />
                      </AuthRoute>
                    }
                  />
                </Route>
                <Route
                  path="/dashboard"
                  element={
                    <PrivateRoute>
                      <UserGuardRoute>
                        <ExamsGuardRoute>
                          <DashboardLayout />
                        </ExamsGuardRoute>
                      </UserGuardRoute>
                    </PrivateRoute>
                  }
                >
                  <Route index element={<Dashboard />} />
                  <Route path="profile" element={<Profile />} />
                  <Route path="exam-management" element={<Exams />} />
                  <Route
                    path="exam-assessment/:examId/:userId"
                    element={<ParticipantAssessPage />}
                  />
                  <Route
                    path="exam-assessment/:examId/:userId/violations"
                    element={<ParticipantViolationsPage />}
                  />
                  <Route path="create-exam" element={<CreateExamPage />} />
                  <Route path="edit-exam/:id" element={<ExamEditPage />} />
                  <Route
                    path="exam-session/:id"
                    element={
                      <ExamSessionProvider>
                        <AntiCheatingProvider>
                          <ExamSession />
                        </AntiCheatingProvider>
                      </ExamSessionProvider>
                    }
                  />
                  <Route
                    path="exam-result/:id"
                    element={
                      <ExamSessionProvider>
                        <StudentExamResult />
                      </ExamSessionProvider>
                    }
                  />
                </Route>
                <Route path="*" element={<Navigate to="/" />} />
                <Route path="/logout" element={<LogoutRoute />} />
              </Routes>
            </ExamsProvider>
          </UserProvider>
        </AuthProvider>
      </NotificationProvider>
    </>
  );
};

export default App;
