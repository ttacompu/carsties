'use client';
import { Pagination } from 'flowbite-react'

type Props ={
    currentPage: number;
    pageCount: number;
    pageChanged: (pageNumber: number) => void;
}
export default function AppPagination({ currentPage, pageCount, pageChanged }: Props) {
  const totalPages = pageCount || 1;
  return (
    <Pagination currentPage={currentPage} 
    onPageChange={e => pageChanged(e)} 
    totalPages={totalPages} 
    layout='pagination' 
    showIcons={true}
    className='text-blue-500 mb-5 '/>
  )
}
