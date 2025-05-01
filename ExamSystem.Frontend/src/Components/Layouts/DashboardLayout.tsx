import { Outlet } from "react-router-dom";
import DashboardNavigation from "../Dashboard/DashboardNavigation.tsx";
import { DashboardProvider } from "../../Providers/DashboardProvider.tsx";
import DashboardOutlet from "../Dashboard/DashboardOutlet.tsx";

const DashboardLayout = () => {
  return (
    <>
      <DashboardProvider>
        <DashboardNavigation />
        <DashboardOutlet>
          <Outlet />
        </DashboardOutlet>
      </DashboardProvider>
    </>
  );
};

export default DashboardLayout;
