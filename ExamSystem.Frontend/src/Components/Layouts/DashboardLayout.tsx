import { Outlet } from 'react-router-dom'
import DashboardNavigation from '../Dashboard/DashboardNavigation.tsx'

const DashboardLayout = () => {
    return (
      <>
        <DashboardNavigation />
        <Outlet />
      </>
    )
  }

export default DashboardLayout