import Search from './Search'
import Logo from './Logo'
import LoginButton from './LoginButton'
import { getCurrentUser } from '../actions/authActions'
import UserActions from './UserActions';

export default async function Navbar() {
  const user = await getCurrentUser();
  return (
    <header className="sticky top-0 z-50 flex h-16 w-full items-center 
    justify-between bg-white px-4 shadow-md">
      <Logo />
      <Search />
      {
        user ? (<UserActions user={user} />) : (<LoginButton />)
      }
    </header>
  )
}
