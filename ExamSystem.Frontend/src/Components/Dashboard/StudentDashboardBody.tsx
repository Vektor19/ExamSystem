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
const StudentDashboardBody: React.FC = () => {
  const { studentCheckedExams, studentExams, removeStudentCheckedExam } =
    useExams();
  const [recentFinishedExams, setRecentFinishedExams] = useState<
    StudentExam[] | null
  >([]);
  const [upcomingExams, setUpcomingExams] = useState<StudentExam[] | null>([]);
  const navigate = useNavigate();

  useEffect(() => {
    if (!studentExams) return;
    const notStartedExams = studentExams.filter((exam) => {
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

    const finishedExams = studentExams.filter((exam) =>
      Boolean(exam.examUser.completeStatus)
    );
    const lastFinishedExams = finishedExams
      .sort((a, b) => {
        const dateA = new Date(a.endDate);
        const dateB = new Date(b.endDate);
        return dateB.getTime() - dateA.getTime();
      })
      .slice(0, 2);

    setRecentFinishedExams(lastFinishedExams);
  }, [studentExams]);

  const handleShowExamResult = (examId: string) => {
    removeStudentCheckedExam(examId);
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
                    onClick={() => handleShowExamResult(exam.examId)}
                  >
                    <ExpandIcon />
                  </PrimaryFab>
                </Stack>
              ))}
          </Stack>
        </DashboardPaper>
        <DashboardPaper>
          <h3>Recent Results</h3>
          <Stack spacing={2} direction={"column"}>
            {recentFinishedExams &&
              recentFinishedExams.length > 0 &&
              recentFinishedExams.map((exam) => (
                <Stack
                  key={exam.examId}
                  direction="row"
                  spacing={2}
                  alignItems="center"
                >
                  {Boolean(exam.examUser.isBlocked) ? (
                    <WarningIcon />
                  ) : exam.examUser.isChecked ? (
                    <CheckIcon />
                  ) : (
                    <TimerIcon />
                  )}
                  <Typography variant="body1">
                    Exam: {exam.name} - Grade: {exam.examUser.grade.toString()}
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
                      size="small"
                      onClick={() => handleShowExamResult(exam.examId)}
                    >
                      <ExpandIcon />
                    </PrimaryFab>
                  </Stack>
                ))}
            {studentCheckedExams?.length === 0 && (
              <Stack
                direction="row"
                spacing={2}
                alignItems="center"
                justifyContent="center"
              >
                <Typography variant="body1">No recent notifications</Typography>
              </Stack>
            )}
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
