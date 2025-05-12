import React, { use, useEffect, useState } from "react";
import studentDashboardStyles from "../../Styles/StudentDashboardBody.module.css";
import DashboardPaper from "../Papers/DashboardPaper";
import { useExams } from "../../Providers/ExamsProvider";
import CheckIcon from "@mui/icons-material/Check";
import WaringIcon from "@mui/icons-material/Warning";
import ExpandIcon from "@mui/icons-material/ExpandMore";

import { Stack, Typography } from "@mui/material";
import { StudentExam } from "../../Models/StudentExam";
import PrimaryFab from "../Buttons/PrimaryFab";
import { useNavigate } from "react-router-dom";
import LoadingPage from "../Extra/LoadingPage";
const StudentDashboardBody: React.FC = () => {
  const { studentCheckedExams, studentExams } = useExams();
  const [upcomingExams, setUpcomingExams] = useState<StudentExam[] | null>([]);
  const navigate = useNavigate();

  useEffect(() => {
    if (!studentExams) return;
    const currentDate = new Date();
    const upcoming = studentExams.filter((exam) => {
      const startDate = new Date(exam.startDate);
      return startDate > currentDate;
    });
    setUpcomingExams(upcoming);
  }, [studentExams]);

  const handleShowExamResult = (examId: string) => {
    navigate(`/dashboard/exam-result/${examId}`);
  };

  if (!studentExams) return <LoadingPage />;

  return (
    <>
      <div className={`${studentDashboardStyles["student-dashboard-body"]}`}>
        <DashboardPaper
          className={`${studentDashboardStyles["upcoming-exams-container"]}`}
        >
          <h3>Upcoming Exams</h3>
          <ul>
            <li>
              Physics — <em>May 15, 2025 at 10:00</em>
            </li>
            <li>
              English — <em>May 20, 2025 at 12:00</em>
            </li>
          </ul>
        </DashboardPaper>
        <DashboardPaper>
          <h3>Recent Results</h3>
          <ul>
            <li>Mathematics — 88%</li>
            <li>Programming — 94%</li>
          </ul>
        </DashboardPaper>
        <DashboardPaper>
          <h3>Notifications</h3>
          <Stack spacing={2} direction={"column"}>
            {studentCheckedExams?.length > 0 &&
              studentExams
                ?.filter((exam) =>
                  studentCheckedExams.some(
                    (checked) => checked.examId === exam.examId
                  )
                )
                .map((exam) => (
                  <Stack
                    key={exam.examId}
                    direction="row"
                    spacing={2}
                    alignItems="center"
                  >
                    <Typography variant="body1">
                      <CheckIcon /> {exam.name} has been checked
                    </Typography>
                    <PrimaryFab
                      onClick={() => handleShowExamResult(exam.examId)}
                    >
                      <ExpandIcon />
                    </PrimaryFab>
                  </Stack>
                ))}
            {upcomingExams &&
              upcomingExams.length > 0 &&
              upcomingExams.map((exam) => (
                <Stack
                  key={exam.examId}
                  direction="row"
                  spacing={2}
                  alignItems="center"
                >
                  <WaringIcon />
                  <Typography variant="body1">
                    Exam: {exam.name} is upcoming
                  </Typography>
                  <PrimaryFab
                    size="small"
                    onClick={() => handleShowExamResult(exam.examId)}
                  >
                    <ExpandIcon />
                  </PrimaryFab>
                </Stack>
              ))}
          </Stack>
        </DashboardPaper>

        <DashboardPaper>
          <h3>Resources</h3>
          <ul>
            <li>📚 Study materials for upcoming exams</li>
            <li>📝 Practice tests and solutions</li>
          </ul>
        </DashboardPaper>
      </div>
    </>
  );
};

export default StudentDashboardBody;
