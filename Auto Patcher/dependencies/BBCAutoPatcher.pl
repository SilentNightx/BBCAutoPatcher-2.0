my $output = open_for_output('BBC_Auto_Patch.esp');
print $output make_header({
    author => 'SilentNightxxx, Greatness7, etc',
    description => 'Better Balanced Combat Auto Patch',
});

my %seen;

my $recordcounter = 0;

for my $plugin (reverse $T3->load_order) {
	# exclude self
    	next if $plugin eq 'BBC_Auto_Patch.esp';

	# exclude base game
	next if $plugin eq 'Morrowind.esm';
	next if $plugin eq 'Tribunal.esm';
	next if $plugin eq 'Bloodmoon.esm';
	
	# exclude mod plugins
	next if $plugin eq 'Better Balanced Combat.esp';
	next if $plugin eq 'Better Balanced Combat Base Module.esp';
	next if $plugin eq 'Better Balanced Combat Effects Module.esp';
	next if $plugin eq 'Better Balanced Combat Extras Module.esp';
	next if $plugin eq 'Better Balanced Combat Weapon Stats Module.esp';
	next if $plugin eq 'Better Balanced Combat Advanced Mode.esp';
	next if $plugin eq 'Better Balanced Combat - Siege at Firemoth Patch.esp';
	next if $plugin eq 'Better Balanced Combat - Tamriel Rebuilt Patch.esp';
	next if $plugin eq 'Better Balanced Combat - TR Patch Base Module.esp';
	next if $plugin eq 'Better Balanced Combat - TR Patch Effects Module.esp';
	next if $plugin eq 'Better Balanced Combat - TR Patch Weapon Stats Module.esp';
	next if $plugin eq 'Better Balanced Combat Rebirth';
	next if $plugin eq 'Better Balanced Combat Rebirth - Base Module.esp';
	next if $plugin eq 'Better Balanced Combat Rebirth - Effects Module.esp';
	next if $plugin eq 'Better Balanced Combat Rebirth - Extras Module.esp';

	# exclude mods with patches
	next if $plugin eq 'Siege at Firemoth.esp';
	next if $plugin eq 'Unofficial Morrowind Official Plugins Patched.ESP';
	next if $plugin eq 'Tamriel_Data.esm';
	next if $plugin eq 'TR_Mainland.esm';
	next if $plugin eq 'TR_Preview.esp';
	next if $plugin eq 'TR_Travels.esp';
	next if $plugin eq 'TR_Factions.esp';
	next if $plugin eq 'Morrowind Rebirth [Main].ESP';
	next if $plugin eq 'Morrowind Rebirth - Birthsigns [Addon].ESP';
	next if $plugin eq 'Morrowind Rebirth - Game Settings [Addon].ESP';
	next if $plugin eq 'Morrowind Rebirth - Mercenaries [Addon].ESP';
	next if $plugin eq 'Morrowind Rebirth - Races [Addon].esp';
	next if $plugin eq 'Morrowind Rebirth - Skills [Addon].ESP';
	next if $plugin eq 'Morrowind Rebirth - Tools [Addon].ESP';
	
	# exclude merged objects patches
	next if $plugin eq 'Merged Objects.esp';
	next if $plugin eq 'Merged_Objects.esp';
	next if $plugin eq 'merged_objects.esp';
	next if $plugin eq 'Merged_Objects1.esp';
	next if $plugin eq 'Merged_Objects_old.esp';
	next if $plugin eq 'Merged_temp.esp';
	next if $plugin eq 'Output.esp';
	
	
	# exclude incompatible mods
	next if $plugin eq 'abotGuars.esp';
	next if $plugin eq 'abotSiltStriders.esp';
	next if $plugin eq 'abotSiltStridersTR1809.esp';
	next if $plugin eq 'abotBoats.esp';
	next if $plugin eq 'abotBoatsTR1809.esp';
	next if $plugin eq 'abotRiverStridersTR1809.esp';
	
    print "Processing Plugin: $plugin\n";

    my $input = open_for_input($plugin);
    while (my $record = TES3::Record->new_from_input($input)) {
        my $type = $record->rectype;
        next if $type !~ /RACE|CREA/;
		
		$recordcounter++;

        my $id = $record->decode->id;
        next if exists $seen{$id}; $seen{$id}++;
        print "Processing Record: $id\n";

        # create the spell
        my $spell = ($type . '::NPCS')->new({spell => 'bbc effects ability'});
        # append the spell
        $record->append($spell);
        # write to output
        $record->encode->write_rec($output)
    }
}

print "Total records patched: $recordcounter\n";

exit;