import React, { useEffect } from "react";
import { Navigate } from "react-router-dom";
import LoadingPage from "../Extra/LoadingPage";
import { useExams } from "../../Providers/ExamsProvider";

const ExamsGuardRoute = ({ children }: { children: React.ReactNode }) => {
  const {
    studentExams,
    examinatorExams,
    isExaminatorExamsLoading,
    isStudentExamsLoading,
    fetchExaminatorExams,
    fetchStudentExams,
  } = useExams();

  useEffect(() => {
    if (!studentExams && !isStudentExamsLoading) {
      fetchStudentExams();
    }
    if (!examinatorExams && !isExaminatorExamsLoading) {
      fetchExaminatorExams();
    }
  }, [
    studentExams,
    examinatorExams,
    isExaminatorExamsLoading,
    isStudentExamsLoading,
  ]);
  if (isStudentExamsLoading || isExaminatorExamsLoading) return <LoadingPage />;
  return studentExams && examinatorExams ? (
    <>{children}</>
  ) : (
    <Navigate to="/login" />
  );
};

export default ExamsGuardRoute;
