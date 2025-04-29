import { Outlet } from 'react-router-dom'
import HomeHeader from '../Home/HomeHeader.tsx'

const DashboardLayout = () => {
    return (
      <>
        <HomeHeader />
        <Outlet />
      </>
    )
  }

export default DashboardLayout