import React, { use, useEffect, useState } from "react";
import studentDashboardStyles from "../../Styles/StudentDashboardBody.module.css";
import DashboardPaper from "../Papers/DashboardPaper";
import { useExams } from "../../Providers/ExamsProvider";
import WarningIcon from "@mui/icons-material/Warning";
import ExpandIcon from "@mui/icons-material/ExpandMore";
import TimerIcon from "@mui/icons-material/Timer";
import PinOutlinedIcon from "@mui/icons-material/PushPinOutlined";

import { IconButton, Stack, Typography } from "@mui/material";
import PrimaryFab from "../Buttons/PrimaryFab";
import { useNavigate } from "react-router-dom";
import LoadingPage from "../Extra/LoadingPage";
import TimeUtils from "../../Utils/TimeUtils";
import { ExaminatorExam } from "../../Models/ExaminatorExam";
const ExaminatorDashboardBody: React.FC = () => {
  const {
    examinatorExams,
    isExaminatorExamsLoading,
    pinnedExaminatorExams,
    fetchPinnedExaminatorExams,
    removePinnedExaminatorExam,
  } = useExams();
  const [notGradedExams, setNotGradedExams] = useState<ExaminatorExam[] | null>(
    []
  );
  const [upcomingExams, setUpcomingExams] = useState<ExaminatorExam[] | null>(
    []
  );
  const [pinnedExams, setPinnedExams] = useState<ExaminatorExam[] | null>([]);
  const navigate = useNavigate();

  useEffect(() => {
    if (!examinatorExams) return;
    fetchPinnedExaminatorExams();
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

  useEffect(() => {
    if (!examinatorExams) return;
    if (!pinnedExaminatorExams) return;
    const pinnedExamsData = examinatorExams.filter((exam) => {
      return pinnedExaminatorExams.some(
        (pinnedExam) => pinnedExam.examId === exam.examId
      );
    });
    setPinnedExams(pinnedExamsData);
  }, [pinnedExaminatorExams]);

  const handleShowExam = (examId: string) => {
    navigate(`/dashboard/edit-exam/${examId}`);
  };

  if (isExaminatorExamsLoading) return <LoadingPage />;

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
          <h3>Pinned exams</h3>
          <Stack spacing={2} direction={"column"}>
            {pinnedExams &&
              pinnedExams.length > 0 &&
              pinnedExams.map((exam) => (
                <Stack
                  key={exam.examId}
                  direction="row"
                  spacing={2}
                  alignItems="center"
                >
                  <IconButton
                    onClick={() => removePinnedExaminatorExam(exam.examId)}
                  >
                    <PinOutlinedIcon />
                  </IconButton>
                  <Typography variant="body1">Exam: {exam.name}</Typography>
                  <PrimaryFab
                    size="small"
                    onClick={() => handleShowExam(exam.examId)}
                  >
                    <ExpandIcon />
                  </PrimaryFab>
                </Stack>
              ))}
            {!pinnedExams ||
              (pinnedExams.length === 0 && (
                <Typography variant="body1">No pinned exams</Typography>
              ))}
          </Stack>
        </DashboardPaper>

        <DashboardPaper>
          <h3>Quick actions</h3>
        </DashboardPaper>
      </div>
    </>
  );
};

export default ExaminatorDashboardBody;
