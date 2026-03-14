'use client'

import { placedBidForAuction } from "@/app/actions/auctionActions";
import { numberWithCommas } from "@/app/lib/numberWithComma";
import { useBidStore } from "@/hooks/useBidsStore";
import { useForm, FieldValues } from "react-hook-form";
import toast from "react-hot-toast";

type Props = {
    auctionId: string;
    highBid: number;
}
export default function BidForm({ auctionId, highBid }: Props) {
    const { register, handleSubmit, reset } = useForm();
    const addBid = useBidStore(state => state.addBid);

    function onSubmit(data: FieldValues) {
        if(data.amount <= highBid) {
            reset();
            toast.error(`Bid must be at least $${ numberWithCommas(highBid + 1) }`);
            return;
        } 
        placedBidForAuction(auctionId, +data.amount)
            .then(bid => {
                if (bid.error) throw bid.error;
                addBid(bid);
                reset();
            }).catch(err => toast.error(err.message))
    }

    return (
        <form onSubmit={handleSubmit(onSubmit)}
            className="flex items-center border-2 rounded-lg py-2">
            <input
                type="number"
                {...register('amount')}
                className="input-custom"
                placeholder={`Enter your bid (minimum bid is $${numberWithCommas(highBid + 1)})`}
            />
        </form>
    )
}
