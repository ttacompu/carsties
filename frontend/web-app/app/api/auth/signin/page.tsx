import EmptyFilter from '@/app/components/EmptyFilter'

export default async  function Page({searchParams}: {searchParams : Promise<{callbackUrl : string}>}) {
const result=await searchParams;
  return (
    <EmptyFilter 
        title='You need to be logged in to do that'
        subTitle='Please click below to sign in'
        showLogin
        callbackUrl={result.callbackUrl}
    />
  )
}
