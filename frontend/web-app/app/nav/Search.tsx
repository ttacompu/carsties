'use client'
import { useParamsStore } from '@/hooks/useParamsStore'
import { ChangeEvent, SyntheticEvent, useRef, useState } from 'react';
import { FaSearch } from 'react-icons/fa'

export default function Search() {
    const setParams = useParamsStore(state => state.setParams);
    const [searchTerm, setSearchTerm] = useState('');
    const inputRef = useRef<HTMLInputElement>(null);

    function handleSubmit(e: SyntheticEvent<HTMLFormElement>) {
        e.preventDefault();
        if (searchTerm.trim()) {
            setParams({ searchTerm: searchTerm.trim() })
        }
    }

    function handleButtonClick() {
        inputRef.current?.focus();
    }

    function handleChange(e: ChangeEvent<HTMLInputElement>) {
        const value = e.target.value;
        setSearchTerm(value);
        if (!value) {
            setParams({ searchTerm: "" });
        }
    }


    return (
        <form onSubmit={handleSubmit} className='flex w-[50%] items-center border-2 border-gray-300 rounded-full py-2 shadow-sm'>
            <input
                ref={inputRef}
                onChange={handleChange}
                type="text"
                placeholder='Search for cars by make, model or color'
                className='grow pl-5 bg-transparent 
            focus:outline-none border-transparent focus:border-transparent focus:ring-0
            text-sm text-gray-600'
            />
            <button onClick={handleButtonClick} >
                <FaSearch size={34}
                    className='bg-red-400 text-white rounded-full p-2 cursor-pointer mx-2' />
            </button>
        </form>
    )
}
