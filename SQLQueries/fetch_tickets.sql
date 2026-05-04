SELECT 
a.id,
title,
b.name as assignee,
case
	when (a.custom_fields->>'1440')::float is null 
		then (a.custom_fields->>'1001')::float 
	else ((a.custom_fields->>'1001')::float - (((a.custom_fields->>'1001')::float * (a.custom_fields->>'1440')::float)/100)) 
end as "Amount After Discount",
(a.custom_fields->>'1240')::float as "Commission",
(select option_value from field_option where id = ((a.custom_fields->>'1226')::int)) as "Payment Frequency"
FROM public.ticket_detail a
join
users b on b.id = a.assigned_to_user_id
join
ticket_status c on a.ticket_status_id = c.id
where
b.name in ('Tonny Odhiambo Ojwang')
and
is_spam is false
and
a.ticket_category_option_id = 3617
order by
a.id