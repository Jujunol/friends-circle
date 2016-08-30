CREATE PROCEDURE geodist @p_lat float, @p_lng float
 
as

-- calculate lon and lat for the rectangle:
declare @lon1 float;
declare @lon2 float;
declare @lat1 float;
declare @lat2 float;
declare @dist float;

set @dist = 100000;

set @lon1 = @p_lng - @dist/abs(cos(radians(@p_lat))*69);
set @lon2 = @p_lng + @dist/abs(cos(radians(@p_lat))*69);
set @lat1 = @p_lat - (@dist/69); 
set @lat2 = @p_lat + (@dist/69);

SELECT friends.*,3956 * 2 * ASIN(SQRT( POWER(SIN((@p_lat -friends.latitude) * pi()/180 / 2), 2) +COS(@p_lat * pi()/180) * COS(friends.latitude * pi()/180) *POWER(SIN((@p_lng -friends.longitude) * pi()/180 / 2), 2) )) as distance 
FROM friends
WHERE longitude between @lon1 and @lon2 
	and latitude between @lat1 and @lat2 
-- having distance < @dist 
ORDER BY Distance;

go
