import { auth } from '@/auth';
import Heading from '../components/Heading';
import Authtest from './Authtest';

export default async function Session() {
  const session = await auth();
  return (
    <div>
     <Heading title="Session dashboard" /> 
     <div className='bg-blue-500 border-2 border-blue-500'>
        <h3 className='text-lg'>Session Data</h3>
        <pre className='whitespace-pre-wrap break-all'>{JSON.stringify(session, null, 2)}</pre>
     </div>
      <div className='mt-4'>
        <Authtest />
      </div>
    </div>
  )
}
