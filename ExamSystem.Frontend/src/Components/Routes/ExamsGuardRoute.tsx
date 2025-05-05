import React, { useEffect } from "react";
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
    if (!studentExams) {
      fetchStudentExams();
    }
    if (!examinatorExams) {
      fetchExaminatorExams();
    }
  }, [
    studentExams,
    examinatorExams,
    isExaminatorExamsLoading,
    isStudentExamsLoading,
  ]);
  if (isStudentExamsLoading || isExaminatorExamsLoading) return <LoadingPage />;
  return <>{children}</>
};

export default ExamsGuardRoute;
