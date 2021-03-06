use stocks

delete from [Stocks].[dbo].[JesseTradingMasterKey]
where JesseTradingMasterKeyId not in (6773, 6774, 9344, 9829, 10857, 13792, 14769, 16171, 16173, 16688, 16870, 
17565, 18659, 18888, 19058, 19306, 19535, 20309,
21226, 21670, 23126, 24959, 25464, 25681, 25682, 27000, 27279, 27712, 28799, 28800, 28801, 32003, 32431, 32800, 33207, 33897,
34322, 34504, 34952, 36567, 37203, 38299, 38882, 39381, 39945, 40039, 40310, 40311, 40673,
40982, 42553, 43119, 43120, 43121, 43122, 43123, 43124, 45548, 46030, 46335)

delete from [Stocks].[dbo].[JesseTradingMasterKeyPivot]

--truncate table [dbo].[ScriptPrice]
select * from [dbo].[Script]
where ScriptId in (
	select [ScriptId] from [Stocks].[dbo].[JesseTradingMasterKey])
order by ScriptName

select * from [dbo].[Script]
where ScriptName = 'BASF' --or ScriptName = 'BAJAJ-AUTO'
select count(*) from [dbo].[ScriptPrice]

select *  FROM [Stocks].[dbo].[JesseTradingMasterKeyPivot] 
SELECT *  FROM [Stocks].[dbo].[JesseTradingMasterKey] 

