import React, { use, useEffect, useState } from "react";
import studentDashboardStyles from "../../Styles/StudentDashboardBody.module.css";
import DashboardPaper from "../Papers/DashboardPaper";
import { useExams } from "../../Providers/ExamsProvider";
import CheckIcon from "@mui/icons-material/Check";
import WarningIcon from "@mui/icons-material/Warning";
import ExpandIcon from "@mui/icons-material/ExpandMore";
import TimerIcon from "@mui/icons-material/Timer";

import { Stack, Typography } from "@mui/material";
import { StudentExam } from "../../Models/StudentExam";
import PrimaryFab from "../Buttons/PrimaryFab";
import { useNavigate } from "react-router-dom";
import LoadingPage from "../Extra/LoadingPage";
import TimeUtils from "../../Utils/TimeUtils";
import { ExaminatorExam } from "../../Models/ExaminatorExam";
const ExaminatorDashboardBody: React.FC = () => {
  const { examinatorExams } = useExams();
  const [notGradedExams, setNotGradedExams] = useState<ExaminatorExam[] | null>(
    []
  );
  const [upcomingExams, setUpcomingExams] = useState<ExaminatorExam[] | null>(
    []
  );
  const navigate = useNavigate();

  useEffect(() => {
    if (!examinatorExams) return;
    const notStartedExams = examinatorExams.filter((exam) => {
      return exam.status === "NotStarted";
    });
    const upcoming = notStartedExams
      .sort((a, b) => {
        const dateA = new Date(a.startDate);
        const dateB = new Date(b.startDate);
        return dateA.getTime() - dateB.getTime();
      })
      .slice(0, 2);
    setUpcomingExams(upcoming);

    const notGradedExams = examinatorExams
      .filter(
        (exam) =>
          exam.participants.some((participant) => !participant.isChecked) &&
          exam.status !== "NotStarted"
      )
      .slice(0, 3);
    setNotGradedExams(notGradedExams);
  }, [examinatorExams]);

  const handleShowExam = (examId: string) => {
    navigate(`/dashboard/edit-exam/${examId}`);
  };

  if (!examinatorExams) return <LoadingPage />;

  return (
    <>
      <div className={`${studentDashboardStyles["student-dashboard-body"]}`}>
        <DashboardPaper
          className={`${studentDashboardStyles["upcoming-exams-container"]}`}
        >
          <h3>Upcoming Exams</h3>
          <Stack spacing={2} direction={"column"}>
            {upcomingExams &&
              upcomingExams.length > 0 &&
              upcomingExams.map((exam) => (
                <Stack
                  key={exam.examId}
                  direction="row"
                  spacing={2}
                  alignItems="center"
                >
                  <WarningIcon />
                  <Typography variant="body1">
                    Exam: {exam.name} is upcoming -{" "}
                    {TimeUtils.formatDate(exam.startDate)}
                  </Typography>
                  <PrimaryFab
                    size="small"
                    onClick={() => handleShowExam(exam.examId)}
                  >
                    <ExpandIcon />
                  </PrimaryFab>
                </Stack>
              ))}
          </Stack>
        </DashboardPaper>
        <DashboardPaper>
          <h3>Not graded exams</h3>
          <Stack spacing={2} direction={"column"}>
            {notGradedExams &&
              notGradedExams.length > 0 &&
              notGradedExams.map((exam) => (
                <Stack
                  key={exam.examId}
                  direction="row"
                  spacing={2}
                  alignItems="center"
                >
                  <TimerIcon />
                  <Typography variant="body1">
                    Exam: {exam.name} is not graded
                  </Typography>
                  <PrimaryFab
                    size="small"
                    onClick={() => handleShowExam(exam.examId)}
                  >
                    <ExpandIcon />
                  </PrimaryFab>
                </Stack>
              ))}
          </Stack>
        </DashboardPaper>
        <DashboardPaper>
          <h3>Notifications</h3>
          <Stack spacing={2} direction={"column"}>
            <Typography variant="body1">No recent notifications</Typography>
          </Stack>
        </DashboardPaper>

        <DashboardPaper>
          <h3>Something</h3>
        </DashboardPaper>
      </div>
    </>
  );
};

export default ExaminatorDashboardBody;
