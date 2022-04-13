<template>
    <h3>Pokoj {{ room.roomNumber }}</h3>
    <p>Počet postelí: {{ room.numberOfBeds }}</p>
    <p>Počet přistýlek: {{ room.numberOfSideBeds }}</p>
    <p>Velikost: {{ room.roomSize }}m<sup>2</sup></p>
    <p>Patro: {{ room.floor }}</p>
</template>
<script lang="js">
    export default {
        name: 'Room',
        props: ['room']
    };
</script>